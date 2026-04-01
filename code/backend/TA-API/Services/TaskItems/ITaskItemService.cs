using TA_API.Models;
using TA_API.Models.Requests;
using TA_API.Models.Responses;

namespace TA_API.Services.TaskItems
{
    /// <summary>
    /// Service interface for task item operations.
    /// </summary>
    public interface ITaskItemService
    {
        Task<TaskItemResponse> CreateAsync(CreateTaskItemRequest request);
        Task<TaskItemResponse?> GetByIdAsync(int id);
        Task<List<TaskItemResponse>> GetAllAsync();
        Task<List<TaskItemResponse>> GetByUserIdAsync(string userId);
        Task<TaskItemResponse?> UpdateAsync(int id, UpdateTaskItemRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
