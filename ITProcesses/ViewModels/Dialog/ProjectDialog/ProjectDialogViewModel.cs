using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Automation;
using System.Windows.Input;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class ProjectDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _dialogProvider;
    private readonly ITaskService _taskService = new TaskService();
    
    private BaseViewModel? _currentChildView;
    
    private ObservableCollection<Project> _projectsList = null!;
    private Project? _selectedProject;
    
    private string _searchBox;
    private List<Project> _projects;
    private List<Project> _allProjects;

    #endregion

    #region Properties
    
    public BaseViewModel? CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<Project> ProjectsList
    {
        get => _projectsList;
        set
        {
            _projectsList = value;
            OnPropertyChanged();
        }
    }
    public Project? SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            OnPropertyChanged();
        }
    }
    
    public string SearchBox
    {
        get => _searchBox;
        set
        {
            _searchBox = value;
            OnPropertyChanged();
            SearchInfoFromSearchBox();
        }
    }
    
    #endregion
    
    //Commands
    public CommandHandler CreateProjectCommand => new (_ => ChangeView(new CreateProjectViewModel(this)));
    public CommandHandler EditProjectCommand => new (_ => ChangeView(null), _ => SelectedProject != null);
    public CommandHandler DeleteProjectCommand => new (_ => ChangeView(null), _ => SelectedProject != null);
    public CommandHandler AcceptSelectCommand => new (_ => AcceptSelectExecute(), _ => SelectedProject != null);
    
    //Constructor
    public ProjectDialogViewModel(DialogProvider dialogProvider)
    {
        _dialogProvider = dialogProvider;
        GetData();
    }
    
    //Methods
    private async void GetData()
    {
        _allProjects = await _taskService.GetAllProject();
        ProjectsList = new ObservableCollection<Project>(_allProjects);
    }
    
    private void AcceptSelectExecute()
    {
        _dialogProvider.CloseDialog(SelectedProject);
    }
    
    private void SearchInfoFromSearchBox()
    {
       
            
        if (!string.IsNullOrEmpty(_searchBox))
        {
            _projects = new List<Project>(_allProjects
                .Where(a => a.Name.ToLower().Contains(_searchBox.ToLower())));


            ProjectsList = new ObservableCollection<Project>(_projects);
        }
        else
        {
            GetData();
        }
    }

    
    public void ChangeView(BaseViewModel? selectedView)
    {
        CurrentChildView = selectedView;
    }
}