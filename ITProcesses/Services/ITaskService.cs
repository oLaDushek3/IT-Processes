using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface ITaskService
{
    Task<List<UsersTask>> GetTasksThisUser(Guid guid);

    Task<Archive> ArchivedTask(Tasks tasks);

    Task<Tasks> CreateTask(Tasks tasks);
}