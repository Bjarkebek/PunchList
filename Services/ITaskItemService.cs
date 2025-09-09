using PunchList.Components.Pages;
using PunchList.Models;

namespace PunchList.Services
{
    public interface ITaskItemService
    {
        Task<List<TaskItem>> LoadTasks();
        Task<TaskItem> CreateTask(TaskItem taskItem);
        Task DeleteTask(int id);
        Task CompleteTask(int id, string uId);
        Task ReopenTask(int id);

        Task ToggleSubTaskAsync(int taskItemId, int subTaskId);
    }
}
