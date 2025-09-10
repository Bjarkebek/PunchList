using Microsoft.EntityFrameworkCore;
using PunchList.Data;
using PunchList.Models;

namespace PunchList.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ApplicationDbContext _db;

        public TaskItemService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TaskItem>> LoadTasks()
        {
            return await _db.TaskItems
                .AsNoTracking()
                .Include(t => t.SubTasks)
                .OrderBy(t => t.Order)
                .ThenBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TaskItem> CreateTask(TaskItem taskItem)
        {
            _db.TaskItems.Add(taskItem);
            await _db.SaveChangesAsync();
            return taskItem;
        }

        public async Task UpdateTask(int id, string title, string? description, DateTime? dueDate)
        {
            var t = await _db.TaskItems.FindAsync(id);
            if (t is null) return;

            t.Title = title;
            t.Description = string.IsNullOrWhiteSpace(description) ? null : description;
            t.DueDate = dueDate;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteTask(int id)
        {
            var t = await _db.TaskItems.FindAsync(id);
            if (t is null) return;
            _db.TaskItems.Remove(t);
            await _db.SaveChangesAsync();
        }

        public async Task CompleteTask(int id, string uId)
        {
            var t = await _db.TaskItems.FindAsync(id);
            if (t is null) return;
            t.Status = PunchList.Models.TaskStatus.Completed;
            t.CompletedByUserId = uId;
            t.CompletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task ReopenTask(int id)
        {
            var t = await _db.TaskItems.FindAsync(id);
            if (t is null) return;
            t.Status = PunchList.Models.TaskStatus.InProgress;
            t.CompletedByUserId = null;
            t.CompletedAt = null;
            await _db.SaveChangesAsync();
        }
    }
}
