using Microsoft.EntityFrameworkCore;
using PunchList.Data;
using PunchList.Models;

namespace PunchList.Services
{
    public class SubTaskItemService : ISubTaskItemService
    {
        private readonly ApplicationDbContext _db;
        public SubTaskItemService(ApplicationDbContext db) => _db = db;

        // Gets subtasks for a task
        public async Task<List<SubTaskItem>> GetForTaskAsync(int taskItemId)
        {
            return await _db.SubTaskItems
                .Where(s => s.TaskItemId == taskItemId)
                .OrderBy(s => s.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        // Finds a subtask by id
        public async Task<SubTaskItem?> GetAsync(int id)
        {
            return await _db.SubTaskItems.FindAsync(id);
        }

        // Creates a new subtask
        public async Task<SubTaskItem> CreateAsync(int taskItemId, string title, int? order = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            var exists = await _db.TaskItems.AnyAsync(t => t.Id == taskItemId);
            if (!exists) throw new InvalidOperationException("Parent TaskItem not found.");

            var next = order ?? ((await _db.SubTaskItems
                                       .Where(s => s.TaskItemId == taskItemId)
                                       .MaxAsync(s => (int?)s.Order)) ?? 0) + 1;

            var st = new SubTaskItem
            {
                TaskItemId = taskItemId,
                Title = title.Trim(),
                IsDone = false,
                Order = next
            };

            _db.SubTaskItems.Add(st);
            await _db.SaveChangesAsync();
            return st;
        }

        // Updates fields on a subtask
        public async Task<SubTaskItem?> UpdateAsync(int id, string? title = null, bool? isDone = null, int? order = null)
        {
            var st = await _db.SubTaskItems.FirstOrDefaultAsync(x => x.Id == id);
            if (st is null) return null;

            if (title != null) st.Title = title.Trim();
            if (isDone.HasValue) st.IsDone = isDone.Value;
            if (order.HasValue) st.Order = order.Value;

            await _db.SaveChangesAsync();
            return st;
        }

        // Deletes a subtask if it exists
        public async Task DeleteAsync(int id)
        {
            var st = await _db.SubTaskItems.FindAsync(id);
            if (st is null) return;
            _db.SubTaskItems.Remove(st);
            await _db.SaveChangesAsync();
        }

        // Toggles the completion state of a subtask
        public async Task ToggleSubTaskAsync(int taskItemId, int subTaskId)
        {
            var st = await _db.SubTaskItems.FirstOrDefaultAsync(x => x.Id == subTaskId && x.TaskItemId == taskItemId);
            if (st is null) return;
            st.IsDone = !st.IsDone;
            await _db.SaveChangesAsync();
        }
    }
}
