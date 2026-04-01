namespace TA_API.Models.Responses
{
    /// <summary>
    /// DTO for task item response.
    /// </summary>
    public class TaskItemResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
    }
}
