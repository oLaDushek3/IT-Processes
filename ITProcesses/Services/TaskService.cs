using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;
using TaskStatus = ITProcesses.Models.TaskStatus;
using Type = ITProcesses.Models.Type;

namespace ITProcesses.Services;

public class TaskService : ITaskService
{
    private readonly ItprocessesContext _context;

    public TaskService(ItprocessesContext context)
    {
        _context = context;
    }

    public async Task<List<Tasks>> GetAllTask()
    {
        return await _context.Tasks.AsNoTracking().Where(t => t.Deleted != true).Include(t => t.Status)
            .Include(t => t.Type)
            .Include(t => t.TaskTags)
            .ThenInclude(tt => tt.Tag).ToListAsync();
    }

    public async Task<List<Tasks>> GetTasksByProject(int id)
    {
        var tasks = await _context.Tasks.AsNoTracking().Include(t => t.Status)
            .Include(t => t.Type)
            .Include(t => t.TaskTags)
            .ThenInclude(tt => tt.Tag)
            .Where(t => t.ProjectId == id && t.Deleted != true).ToListAsync();

        if (tasks == null)
            throw new Exception("Проект не найден");

        return tasks;
    }

    public async Task<List<Tasks>> GetTasksThisUser(Guid userId)
    {
        // var tasks = await _context.Tasks.
        //     Include(t => t.UsersTasks).
        //     Where(t => t.UsersTasks.FirstOrDefault(c => c.UserId == userId) != null).
        //     ToListAsync();
        //
        // foreach (var task in tasks)
        // {
        //     var t = task;
        //     while (t?.BeforeTask != null)
        //     {
        //         t.BeforeTaskNavigation = await _context.Tasks.FirstOrDefaultAsync(e => e.Id == t.BeforeTask);
        //         t = t.BeforeTaskNavigation;
        //     }
        // }

        List<Tasks> result = new();
        var userTasks = await _context.UsersTasks.Where(ut => ut.UserId == userId).ToListAsync();
        foreach (var userTask in userTasks)
        {
            Tasks? task = _context.Tasks.Include(t => t.InverseBeforeTaskNavigation)
                .ThenInclude(t => t.InverseBeforeTaskNavigation).ThenInclude(t => t.InverseBeforeTaskNavigation)
                .ThenInclude(t => t.InverseBeforeTaskNavigation).ThenInclude(t => t.InverseBeforeTaskNavigation)
                .FirstOrDefault(t => t.Id == userTask.TaskId && t.BeforeTask == null && t.Deleted != true);
            if (task != null)
                result.Add(task);
        }

        return result;
    }

    public async Task<Tasks> GetTaskById(Guid guid)
    {
        var tasks = await _context.Tasks.AsNoTracking().Where(t => t.Id == guid && t.Deleted != true).Include(t => t.UsersTasks)
            .ThenInclude(ut => ut.User)
            .ThenInclude(u => u.Role).Include(t => t.TaskDocuments).ThenInclude(td => td.Documents)
            .Include(t => t.TaskTags).ThenInclude(tt => tt.Tag).Include(t => t.Status).FirstAsync();

        if (tasks == null)
            throw new Exception("Задача не найдена");

        return tasks;
    }

    public async Task<Tasks> CreateTask(Tasks tasks)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == tasks.Id);

        if (task != null)
            throw new Exception("Данная задача уже существует!");

        tasks.Id = Guid.NewGuid();
        await _context.Tasks.AddAsync(tasks);
        await _context.SaveChangesAsync();

        return tasks;
    }

    public async Task DeleteTask(Tasks tasks)
    {
        tasks.Deleted = true;
        _context.Tasks.Update(tasks);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TaskStatus>> GetAllStatuses()
    {
        return await _context.TaskStatuses.ToListAsync();
    }

    public async Task<List<Type>> GetAllTypes()
    {
        return await _context.Types.ToListAsync();
    }

    public async Task<List<UsersTask>> GetAllUsersFromTask(Guid guid)
    {
        var users = await _context.UsersTasks
            .Include(u => u.User)
            .Where(u => u.TaskId == guid).ToListAsync();

        if (users == null)
            throw new Exception("Пользователи не найдены");

        return users;
    }

    public async Task<Tasks> UpdateTask(Tasks tasks)
    {
        _context.Tasks.Update(tasks);
        await _context.SaveChangesAsync();
        return tasks;
    }

    public async Task DeleteUsersTask(UsersTask usersTask)
    {
        _context.UsersTasks.Remove(usersTask);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskDocument(TaskDocument taskDocument)
    {
        _context.Documents.Remove(taskDocument.Documents);
        _context.TaskDocuments.Remove(taskDocument);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersNotParticipatingInTask(Guid taskId)
    {
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        var usersTasks = await _context.UsersTasks.Where(us => us.TaskId == taskId).ToListAsync();

        foreach (var usersTask in usersTasks)
        {
            users.Remove(usersTask.User!);
        }

        return users;
    }
}