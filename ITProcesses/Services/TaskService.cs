using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Services;

public class TaskService : BaseViewModel, ITaskService
{
    public async Task<List<UsersTask>> GetTasksThisUser(User user)
    {
        var tasks = await Context.UsersTasks.Include(c => c.Task)
            .Where(c => c.UserId == user.Id).ToListAsync();

        if (tasks == null)
            throw new Exception("Не найден пользователь");

        return tasks;
    }
}