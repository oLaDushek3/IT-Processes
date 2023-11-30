using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITProcesses.Models;
using ITProcesses.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Services;

public class ProjectService : IProjectService
{
    private readonly ItprocessesContext _context;
    public ProjectService(ItprocessesContext context)
    {
        _context = context;
    }
    
    public async Task<Project> CreateProject(Project project)
    {
        var proj = await _context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
        
        if (proj != null)
            throw new Exception("Данный проект уже существует!");
        
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        
        return project;
    }

    public async Task DeleteProject(Project project)
    {
        var tasksList = _context.Tasks.Where(t => t.ProjectId == project.Id);
        foreach (var tasks in tasksList)
        {
            tasks.TaskDocuments = null;
            tasks.InverseBeforeTaskNavigation = null;
            tasks.UsersTasks = null;
            tasks.TaskTags = null;
            _context.Tasks.Update(tasks);
            await _context.SaveChangesAsync();
            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();
        }

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project> EditProject(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }
    
    public async Task<List<Project>> GetAllProject()
    {
        return await _context.Projects.Include(p => p.Tasks).ToListAsync();
    }

    public async Task<Project> GetProjectById(int id)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            throw new Exception("Проект не найдена");

        return project;
    }
}