using System.Collections.Generic;
using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface IProjectService
{
    Task<Project> CreateProject(Project project);

    void DeleteProject(Project project);

    Task<Project> EditProject(Project project);
    public Task<List<Project>> GetAllProject();
    public Task<Project> GetProjectById(int id);
}