using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Services;

public class ProjectService : BaseViewModel, IProjectService
{
    public async Task<Project> CreateProject(Project project)
    {
        var proj = await Context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);

        if (proj != null)
            throw new Exception("Данный проект уже существует!");

        await Context.Projects.AddAsync(project);
        await Context.SaveChangesAsync();

        return project;
    }

    public async void DeleteProject(Project project)
    {
        var tasksList = Context.Tasks.Where(t => t.ProjectId == project.Id);
        foreach (var tasks in tasksList)
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

        Context.Projects.Update(project);
        await Context.SaveChangesAsync();
    }

    public async Task<Project> EditProject(Project project)
    {
        Context.Projects.Update(project);
        await Context.SaveChangesAsync();
        return project;
    }

    public async Task<List<Project>> GetAllProject()
    {
        var project = await Context.Projects.Include(p => p.Tasks).ToListAsync();

        return project;
    }

    public async Task<Project> GetProjectById(int id)
    {
        var project = await Context.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            throw new Exception("Проект не найдена");

        return project;
    }
}