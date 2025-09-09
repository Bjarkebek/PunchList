using PunchList.Models;

namespace PunchList.Services
{
    public interface ISubTaskItemService
    {
        Task<List<SubTaskItem>> GetForTaskAsync(int taskItemId);
        Task<SubTaskItem?> GetAsync(int id);
        Task<SubTaskItem> CreateAsync(int taskItemId, string title, int? order = null);
        Task<SubTaskItem?> UpdateAsync(int id, string? title = null, bool? isDone = null, int? order = null);
        Task DeleteAsync(int id);

        

    }
}
