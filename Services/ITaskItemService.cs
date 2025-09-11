using PunchList.Models;

namespace PunchList.Services
{
    public interface ITaskItemService
    {
        Task<List<TaskItem>> LoadTasks();
        Task<TaskItem> CreateTask(TaskItem taskItem);
        Task DeleteTask(int id);
        Task UpdateTask(int id, string title, string? description, DateTime? dueDate);
        Task CompleteTask(int id, string uId);
        Task ReopenTask(int id);
    }
}
