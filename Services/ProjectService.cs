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

        public async Task UpdateProjectAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Project project)
        {
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }
    }
}
