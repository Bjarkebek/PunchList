using PunchList.Models;

namespace PunchList.Services
{
    public interface IProjectService
    {
        // CRUD operations for Project entity
        Task<List<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project Project);
        Task UpdateProjectAsync(Project Project);
        Task DeleteProjectAsync(Project Project);
    }
}
