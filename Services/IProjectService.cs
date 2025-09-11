using PunchList.Models;

namespace PunchList.Services
{
    public interface IProjectService
    {
        // All projects (active + archived)
        Task<List<Project>> GetProjectsAsync();

        // Active (not completed) projects
        Task<List<Project>> GetActiveProjectsAsync();

        // Archived (completed) projects
        Task<List<Project>> GetArchivedProjectsAsync();

        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);

        // Completion ops
        Task CompleteProjectAsync(int id);
        Task ReopenProjectAsync(int id);
    }
}
