using System.ComponentModel.DataAnnotations;

namespace PunchList.Models;

public class Project
{
    [Key]
    public int Id { get; set; } //PK
    [Required, MaxLength(150)]
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public DateTime CompletedAt { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();
}
