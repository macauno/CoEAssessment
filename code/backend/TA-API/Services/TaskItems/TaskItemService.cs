using TA_API.Models;
using TA_API.Models.Requests;
using TA_API.Models.Responses;
using TA_API.Services.Data;

namespace TA_API.Services.TaskItems
{
    /// <summary>
    /// Service for managing task items.
    /// </summary>
    public class TaskItemService : ITaskItemService
    {
        private readonly AssessmentDbContext _dbContext;
        private readonly ILogger<TaskItemService> _logger;

        public TaskItemService(AssessmentDbContext dbContext, ILogger<TaskItemService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TaskItemResponse> CreateAsync(CreateTaskItemRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    throw new ArgumentException("Title cannot be empty.");
                if (string.IsNullOrWhiteSpace(request.Description))
                    throw new ArgumentException("Description cannot be empty.");
                if (string.IsNullOrWhiteSpace(request.UserId))
                    throw new ArgumentException("UserId cannot be empty.");

                var taskItem = new TaskItem
                {
                    Title = request.Title,
                    Description = request.Description,
                    UserId = request.UserId,
                    IsCompleted = false
                };

                _dbContext.TaskItems.Add(taskItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"TaskItem created with ID: {taskItem.Id}");
                return MapToResponse(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task item: {ex.Message}");
                throw;
            }
        }

        public async Task<TaskItemResponse?> GetByIdAsync(int id)
        {
            try
            {
                var taskItem = await _dbContext.TaskItems.FindAsync(id);
                if (taskItem == null)
                {
                    _logger.LogWarning($"TaskItem with ID {id} not found.");
                    return null;
                }

                return MapToResponse(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task item: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TaskItemResponse>> GetAllAsync()
        {
            try
            {
                var taskItems = _dbContext.TaskItems.ToList();
                return taskItems.Select(MapToResponse).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all task items: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TaskItemResponse>> GetByUserIdAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("UserId cannot be empty.");

                var taskItems = _dbContext.TaskItems.Where(t => t.UserId == userId).ToList();
                return taskItems.Select(MapToResponse).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task items for user {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<TaskItemResponse?> UpdateAsync(int id, UpdateTaskItemRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    throw new ArgumentException("Title cannot be empty.");
                if (string.IsNullOrWhiteSpace(request.Description))
                    throw new ArgumentException("Description cannot be empty.");

                var taskItem = await _dbContext.TaskItems.FindAsync(id);
                if (taskItem == null)
                {
                    _logger.LogWarning($"TaskItem with ID {id} not found for update.");
                    return null;
                }

                taskItem.Title = request.Title;
                taskItem.Description = request.Description;
                taskItem.IsCompleted = request.IsCompleted;

                _dbContext.TaskItems.Update(taskItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"TaskItem with ID {id} updated successfully.");
                return MapToResponse(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task item: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var taskItem = await _dbContext.TaskItems.FindAsync(id);
                if (taskItem == null)
                {
                    _logger.LogWarning($"TaskItem with ID {id} not found for deletion.");
                    return false;
                }

                _dbContext.TaskItems.Remove(taskItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"TaskItem with ID {id} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task item: {ex.Message}");
                throw;
            }
        }

        private static TaskItemResponse MapToResponse(TaskItem taskItem)
        {
            return new TaskItemResponse
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                UserId = taskItem.UserId
            };
        }
    }
}
