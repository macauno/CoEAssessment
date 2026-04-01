namespace TA_API.Models.Requests
{
    /// <summary>
    /// DTO for creating a new task item.
    /// </summary>
    public class CreateTaskItemRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserId { get; set; }
    }
}
