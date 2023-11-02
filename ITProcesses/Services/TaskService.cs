using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;
using TaskStatus = ITProcesses.Models.TaskStatus;

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

    public async Task<Tasks> CreateTask(Tasks tasks)
    {
        var task = await Context.Tasks.FirstOrDefaultAsync(t => t.Id == tasks.Id);

        if (task != null)
            throw new Exception("Данная задача уже существует!");

        await Context.Tasks.AddAsync(tasks);
        await Context.SaveChangesAsync();

        return tasks;
    }

    public async Task<Project> CreateProject(Project project)
    {
        var proj = await Context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

        if (proj != null)
            throw new Exception("Данный проект уже существует!");

        await Context.Projects.AddAsync(project);
        await Context.SaveChangesAsync();

        return project;
    }

    public async Task<List<TaskStatus>> GetAllStatuses()
    {
        return await Context.TaskStatuses.ToListAsync();
    }

    public async Task<List<UsersTask>> GetAllUsersFromTask(Guid guid)
    {
        var users = await Context.UsersTasks.Include(u => u.User)
            .Where(u => u.TaskId == guid).ToListAsync();

        if (users == null)
            throw new Exception("Пользователи не найдены");

        return users;
    }

    public async Task<List<Tasks>> GetAllTask()
    {
        return await Context.Tasks.ToListAsync();
    }

    public async void DeleteTask(Tasks tasks)
    {
        tasks.TaskDocuments = null;
        tasks.InverseBeforeTaskNavigation = null;
        tasks.UsersTasks = null;
        tasks.TaskTags = null;
        Context.Tasks.Update(tasks);
        await Context.SaveChangesAsync();
        Context.Tasks.Remove(tasks);
        await Context.SaveChangesAsync();
    }
}