using System.ComponentModel.DataAnnotations;

namespace PunchList.Models;

public enum TaskStatus
{
    New,
    InProgress,
    Completed
}

public class TaskItem
{
    [Key]
    public int Id { get; set; } //PK
    public int ProjectId { get; set; } //FK
    [Required, MaxLength(200)]
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.New;
    public DateTime? DueDate { get; set; }
    public int Order { get; set; } = 0;
    public List<SubTaskItem> SubTasks { get; set; } = new();
    public string? CompletedByUserId { get; set; }
    public DateTime? CompletedAt { get; set; }
}
