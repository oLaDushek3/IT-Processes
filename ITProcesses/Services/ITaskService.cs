using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITProcesses.Models;
using TaskStatus = ITProcesses.Models.TaskStatus;

namespace ITProcesses.Services;

public interface ITaskService
{
    Task<List<UsersTask>> GetTasksThisUser(Guid guid);
    
    Task<Tasks> CreateTask(Tasks tasks);

    Task<Project> CreateProject(Project project);

    Task<List<TaskStatus>> GetAllStatuses();
}