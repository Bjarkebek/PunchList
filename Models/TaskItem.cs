using PunchList.Models;
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
    public int Id { get; set; } //Pk

    public int ProjectId { get; set; } //Fk

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
