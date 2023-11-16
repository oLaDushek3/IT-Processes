using System;
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

    public void DeleteProject(Project project)
    {
    }

    public async Task<Project> EditProject(Project project)
    {
        Context.Projects.Update(project);
        await Context.SaveChangesAsync();
        return project;
    }
}