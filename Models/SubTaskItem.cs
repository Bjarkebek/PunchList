using System.ComponentModel.DataAnnotations;

namespace PunchList.Models;

public class SubTaskItem
{
    [Key]
    public int Id { get; set; } //PK
    public int TaskItemId { get; set; } //FK
    [Required, MaxLength(200)]
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
    public int Order { get; set; } = 0;
}
