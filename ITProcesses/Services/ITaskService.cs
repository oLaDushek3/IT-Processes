using System.Collections.Generic;
using System.Threading.Tasks;
using ITProcesses.Models;

namespace ITProcesses.Services;

public interface ITaskService
{
    Task<List<UsersTask>> GetTasksThisUser(User user);
}