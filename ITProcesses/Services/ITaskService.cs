using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using ITProcesses.Models;
using TaskStatus = ITProcesses.Models.TaskStatus;
using Type = ITProcesses.Models.Type;

namespace ITProcesses.Services;

public interface ITaskService
{
    Task<List<Tasks>> GetTasksThisUser(Guid userId);

    Task<List<Tasks>> GetTasksByProject(int id);
    
    Task<Tasks> GetTaskById(Guid guid);
    
    Task<Tasks> CreateTask(Tasks tasks);
    
    Task<List<TaskStatus>> GetAllStatuses();

    Task<List<Type>> GetAllTypes();
    
    Task<List<UsersTask>> GetAllUsersFromTask(Guid guid);

    Task<List<Tasks>> GetAllTask();

    Task DeleteTask(Tasks tasks);

    Task<Tasks> UpdateTask(Tasks tasks);
    
    Task DeleteUsersTask(UsersTask usersTask);
    
    Task DeleteTaskDocument(TaskDocument taskDocument);

    Task<List<User>> GetUsersNotParticipatingInTask(Guid taskId);
}