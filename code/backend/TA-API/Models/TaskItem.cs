namespace TA_API.Models
{
    /// <summary>
    /// Domain entity representing a task item.
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string UserId { get; set; }
    }
}
