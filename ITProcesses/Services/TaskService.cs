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
    public async Task<List<UsersTask>> GetTasksThisUser(Guid guid)
    {
        var tasks = await Context.UsersTasks.Include(c => c.Task)
            .Where(c => c.UserId == guid).ToListAsync();

        if (tasks == null)
            throw new Exception("Не найден пользователь");

        return tasks;
    }

    public async Task<Archive> ArchivedTask(Tasks tasks)
    {
        Archive archive = new Archive();

        if (tasks.Archived == true)
        {
            archive.Task = tasks;

            await Context.Archives.AddAsync(archive);
            await Context.SaveChangesAsync();
        }
        else
        {
            tasks.Archived = true;
            Context.Tasks.Update(tasks);

            archive.Task = tasks;

            await Context.Archives.AddAsync(archive);
            await Context.SaveChangesAsync();
        }

        return archive;
    }

    public Task<Tasks> CreateTask(Tasks tasks)
    {
        throw new NotImplementedException();
    }
}