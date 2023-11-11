using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<List<Tasks>> GetTasksByProject(int id)
    {
        var tasks = await Context.Tasks.Where(t => t.ProjectId == id).ToListAsync();

        if (tasks == null)
            throw new Exception("Проект не найден");

        return tasks;
    }

    public async Task<Tasks> GetTaskById(Guid guid)
    {
        var tasks = await Context.Tasks.Where(t => t.Id == guid).Include(t => t.UsersTasks).ThenInclude(ut => ut.User)
            .ThenInclude(u => u.Role).Include(t => t.TaskDocuments).ThenInclude(td => td.Documents)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag).Include(t => t.Status).FirstAsync();

        if (tasks == null)
            throw new Exception("Задача не найдена");

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
        var users = await Context.UsersTasks
            .Include(u => u.User)
            .Where(u => u.TaskId == guid).ToListAsync();

        if (users == null)
            throw new Exception("Пользователи не найдены");

        return users;
    }

    public async Task<List<Tasks>> GetAllTask()
    {
        return await Context.Tasks.Include(t => t.Status)
            .Include(t => t.Type)
            .Include(t => t.TaskTags)
            .ThenInclude(tt => tt.Tag).ToListAsync();
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

    public async Task<Tasks> UpdateTask(Tasks tasks)
    {
        Context.Tasks.Update(tasks);
        await Context.SaveChangesAsync();
        return tasks;
    }
}