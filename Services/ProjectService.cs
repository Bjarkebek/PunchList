using Microsoft.EntityFrameworkCore;
using PunchList.Data;
using PunchList.Models;

namespace PunchList.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _db;
        public ProjectService(ApplicationDbContext db) => _db = db;

        // Gets all projects (active + archived)
        public async Task<List<Project>> GetProjectsAsync()
        {
            // AsNoTracking for read-only queries; Include to eager-load graph
            return await _db.Projects.AsNoTracking()
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        // Gets only active (not completed) projects
        public async Task<List<Project>> GetActiveProjectsAsync()
        {
            return await _db.Projects.AsNoTracking()
                .Where(p => !p.IsCompleted)
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        // Gets only archived (completed) projects
        public async Task<List<Project>> GetArchivedProjectsAsync()
        {
            return await _db.Projects.AsNoTracking()
                .Where(p => p.IsCompleted)
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .OrderByDescending(p => p.CompletedAt)
                .ToListAsync();
        }

        // Gets a project by id
        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.AsNoTracking()
                .Include(p => p.Tasks).ThenInclude(t => t.SubTasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Adds a new project
        public async Task AddProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        // Updates fields of a project
        public async Task UpdateProjectAsync(Project project)
        {
            var p = await _db.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

            p.Name = project.Name;
            p.Description = project.Description;

            await _db.SaveChangesAsync();
        }

        // Deletes a project
        public async Task DeleteProjectAsync(Project project)
        {
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        // Marks a project as completed
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

        // Reopens a project
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
