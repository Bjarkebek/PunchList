using Microsoft.EntityFrameworkCore;
using PunchList.Data;
using PunchList.Models;

namespace PunchList.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _db;
        public ProjectService(ApplicationDbContext db) => _db = db;

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _db.Projects.AsNoTracking()
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Project>> GetActiveProjectsAsync()
        {
            return await _db.Projects.AsNoTracking()
                .Where(p => !p.IsCompleted)
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Project>> GetArchivedProjectsAsync()
        {
            return await _db.Projects.AsNoTracking()
                .Where(p => p.IsCompleted)
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderByDescending(p => p.CompletedAt)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.AsNoTracking()
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        // Only update fields on the tracked entity to avoid attaching a second instance.
        public async Task UpdateProjectAsync(Project project)
        {
            var existing = await _db.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            if (existing is null)
            {
                // Optionally throw or no-op
                return;
            }

            existing.Name = project.Name;
            existing.Description = project.Description;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Project project)
        {
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        public async Task CompleteProjectAsync(int id)
        {
            var existing = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (existing is null) return;

            if (!existing.IsCompleted)
            {
                existing.IsCompleted = true;
                existing.CompletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReopenProjectAsync(int id)
        {
            var existing = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (existing is null) return;

            if (existing.IsCompleted)
            {
                existing.IsCompleted = false;
                existing.CompletedAt = default; // clear the completion timestamp
                await _db.SaveChangesAsync();
            }
        }
    }
}
