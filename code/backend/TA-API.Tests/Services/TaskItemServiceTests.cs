using NSubstitute;
using Xunit;
using TA_API.Models;
using TA_API.Models.Requests;
using TA_API.Models.Responses;
using TA_API.Services.Data;
using TA_API.Services.TaskItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TA_API.Tests.Services
{
    public class TaskItemServiceTests : IDisposable
    {
        private readonly AssessmentDbContext _dbContext;
        private readonly ILogger<TaskItemService> _mockLogger;
        private readonly TaskItemService _service;

        public TaskItemServiceTests()
        {
            var options = new DbContextOptionsBuilder<AssessmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AssessmentDbContext(options);
            _mockLogger = Substitute.For<ILogger<TaskItemService>>();
            _service = new TaskItemService(_dbContext, _mockLogger);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region CreateAsync Tests

        [Fact]
        public async Task CreateAsync_WithValidRequest_ShouldReturnTaskItemResponse()
        {
            // Arrange
            var request = new CreateTaskItemRequest
            {
                Title = "Test Task",
                Description = "Test Description",
                UserId = "user123"
            };

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal("user123", result.UserId);
            Assert.False(result.IsCompleted);
        }

        [Fact]
        public async Task CreateAsync_WithNullTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new CreateTaskItemRequest
            {
                Title = "",
                Description = "Test Description",
                UserId = "user123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_WithNullDescription_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new CreateTaskItemRequest
            {
                Title = "Test Task",
                Description = "",
                UserId = "user123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new CreateTaskItemRequest
            {
                Title = "Test Task",
                Description = "Test Description",
                UserId = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(request));
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnTaskItem()
        {
            // Arrange
            _dbContext.TaskItems.Add(new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                UserId = "user123",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WithMultipleItems_ShouldReturnList()
        {
            // Arrange
            _dbContext.TaskItems.AddRange(
                new TaskItem { Title = "Task 1", Description = "Desc 1", UserId = "user1", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TaskItem { Title = "Task 2", Description = "Desc 2", UserId = "user2", IsCompleted = true, CreatedAt = DateTime.UtcNow }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_WithNoItems_ShouldReturnEmptyList()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetByUserIdAsync Tests

        [Fact]
        public async Task GetByUserIdAsync_WithValidUserId_ShouldReturnUserTasks()
        {
            // Arrange
            _dbContext.TaskItems.AddRange(
                new TaskItem { Title = "Task 1", Description = "Desc 1", UserId = "user123", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TaskItem { Title = "Task 2", Description = "Desc 2", UserId = "user123", IsCompleted = true, CreatedAt = DateTime.UtcNow },
                new TaskItem { Title = "Task 3", Description = "Desc 3", UserId = "other", IsCompleted = false, CreatedAt = DateTime.UtcNow }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetByUserIdAsync("user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByUserIdAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.GetByUserIdAsync(""));
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WithValidRequest_ShouldUpdateAndReturn()
        {
            // Arrange
            _dbContext.TaskItems.Add(new TaskItem
            {
                Title = "Old Title",
                Description = "Old Description",
                UserId = "user123",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            var updateRequest = new UpdateTaskItemRequest
            {
                Title = "New Title",
                Description = "New Description",
                IsCompleted = true
            };

            // Act
            var result = await _service.UpdateAsync(1, updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
            Assert.Equal("New Description", result.Description);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var updateRequest = new UpdateTaskItemRequest
            {
                Title = "New Title",
                Description = "New Description",
                IsCompleted = true
            };

            // Act
            var result = await _service.UpdateAsync(999, updateRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_WithEmptyTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var updateRequest = new UpdateTaskItemRequest
            {
                Title = "",
                Description = "New Description",
                IsCompleted = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAsync(1, updateRequest));
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            _dbContext.TaskItems.Add(new TaskItem
            {
                Title = "Task to Delete",
                Description = "Description",
                UserId = "user123",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Null(await _dbContext.TaskItems.FindAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
