using PunchList.Models;

namespace PunchList.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync();
        Task<List<Project>> GetActiveProjectsAsync();
        Task<List<Project>> GetArchivedProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
        Task CompleteProjectAsync(int id);
        Task ReopenProjectAsync(int id);
    }
}
