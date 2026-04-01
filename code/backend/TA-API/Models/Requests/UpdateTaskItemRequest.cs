namespace TA_API.Models.Requests
{
    /// <summary>
    /// DTO for updating a task item.
    /// </summary>
    public class UpdateTaskItemRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
