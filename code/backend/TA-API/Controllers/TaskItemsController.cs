using Microsoft.AspNetCore.Mvc;
using TA_API.Models.Requests;
using TA_API.Models.Responses;
using TA_API.Services.TaskItems;

namespace TA_API.Controllers
{
    /// <summary>
    /// API controller for task item operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _service;
        private readonly ILogger<TaskItemsController> _logger;

        public TaskItemsController(ITaskItemService service, ILogger<TaskItemsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Create a new task item.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return Ok(ApiResponse<TaskItemResponse>.Success(result, 201));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(ApiResponse<TaskItemResponse>.Failure(ex.Message, 400));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task item: {ex.Message}");
                return StatusCode(500, ApiResponse<TaskItemResponse>.Failure("Internal server error", 500));
            }
        }

        /// <summary>
        /// Get a task item by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<TaskItemResponse>.Failure("Task item not found", 404));

                return Ok(ApiResponse<TaskItemResponse>.Success(result, 200));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task item: {ex.Message}");
                return StatusCode(500, ApiResponse<TaskItemResponse>.Failure("Internal server error", 500));
            }
        }

        /// <summary>
        /// Get all task items.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _service.GetAllAsync();
                return Ok(ApiResponse<List<TaskItemResponse>>.Success(results, 200));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all task items: {ex.Message}");
                return StatusCode(500, ApiResponse<List<TaskItemResponse>>.Failure("Internal server error", 500));
            }
        }

        /// <summary>
        /// Get all task items for a specific user.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var results = await _service.GetByUserIdAsync(userId);
                return Ok(ApiResponse<List<TaskItemResponse>>.Success(results, 200));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(ApiResponse<List<TaskItemResponse>>.Failure(ex.Message, 400));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task items for user: {ex.Message}");
                return StatusCode(500, ApiResponse<List<TaskItemResponse>>.Failure("Internal server error", 500));
            }
        }

        /// <summary>
        /// Update a task item.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);
                if (result == null)
                    return NotFound(ApiResponse<TaskItemResponse>.Failure("Task item not found", 404));

                return Ok(ApiResponse<TaskItemResponse>.Success(result, 200));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(ApiResponse<TaskItemResponse>.Failure(ex.Message, 400));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task item: {ex.Message}");
                return StatusCode(500, ApiResponse<TaskItemResponse>.Failure("Internal server error", 500));
            }
        }

        /// <summary>
        /// Delete a task item.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.Failure("Task item not found", 404));

                return Ok(ApiResponse<object>.Success(new { message = "Task item deleted successfully" }, 200));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task item: {ex.Message}");
                return StatusCode(500, ApiResponse<object>.Failure("Internal server error", 500));
            }
        }
    }
}
