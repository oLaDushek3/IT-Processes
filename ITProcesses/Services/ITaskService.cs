using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using ITProcesses.Models;
using TaskStatus = ITProcesses.Models.TaskStatus;

namespace ITProcesses.Services;

public interface ITaskService
{
    Task<List<UsersTask>> GetTasksThisUser(Guid guid);
    
    Task<Tasks> CreateTask(Tasks tasks);

    Task<Project> CreateProject(Project project);

    Task<List<TaskStatus>> GetAllStatuses();

    Task<List<UsersTask>> GetAllUsersFromTask(Guid guid);

    Task<List<Tasks>> GetAllTask();

    void DeleteTask(Tasks tasks);
}