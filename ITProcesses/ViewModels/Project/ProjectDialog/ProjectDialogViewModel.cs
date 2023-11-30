using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class ProjectDialogViewModel : BaseViewModel
{
    #region Field
    
    public readonly MainViewModel CurrentMainViewModel;
    private readonly DialogProvider _currentDialogProvider;
    
    private readonly ItprocessesContext _context = new();
    private readonly IProjectService _projectService;

    private ObservableCollection<Project> _projectsList = null!;
    private Project? _selectedProject;
    private Project? _currentProject;

    private string _searchBox;
    private List<Project> _projects;
    private List<Project> _allProjects;

    #endregion

    #region Properties

    public DialogProvider ConfirmDialogProvider { get; } = new();

    public DialogProvider CreateEditDialogProvider { get; } = new();

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
    public CommandHandler AcceptSelectCommand =>
        new(_ => _currentDialogProvider.CloseDialog(SelectedProject), _ => SelectedProject != null);

    public CommandHandler CancelCommand =>
        new(_ => _currentDialogProvider.CloseDialog(null), _ => _currentProject != null);

    public CommandHandler CreateProjectCommand => new(_ => CreateProjectCommandExecute());

    public CommandHandler EditProjectCommand =>
        new(_ => EditProjectCommandExecute(), _ => SelectedProject != null);

    public CommandHandler DeleteProjectCommand => 
        new(_ => DeleteProjectCommandExecute(), _ => SelectedProject != null);

    //Constructor
    public ProjectDialogViewModel(DialogProvider currentDialogProvider, MainViewModel currentMainViewModel, Project currentProject)
    {
        _projectService = new ProjectService(_context);
        _currentDialogProvider = currentDialogProvider;
        CurrentMainViewModel = currentMainViewModel;
        _currentProject = currentProject;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        _allProjects = await _projectService.GetAllProject();
        ProjectsList = new ObservableCollection<Project>(_allProjects);
    }

    private async void CreateProjectCommandExecute()
    {
        var dialogResult =
            (Project?)await CreateEditDialogProvider.ShowDialog(
                new CreateProjectViewModel(this, CreateEditDialogProvider));

        if (dialogResult == null) return;

        _allProjects.Add(dialogResult);
        ProjectsList = new ObservableCollection<Project>(_allProjects);
        SelectedProject = dialogResult;
    }

    private async void EditProjectCommandExecute()
    {
        var dialogResult =
            (Project?)await CreateEditDialogProvider.ShowDialog(new EditProjectDialogViewModel(SelectedProject!,
                CreateEditDialogProvider));

        if (dialogResult == null) return;

        _allProjects[_allProjects.FindIndex(p => p.Id == SelectedProject!.Id)] = dialogResult;
        ProjectsList = new ObservableCollection<Project>(_allProjects);
        SelectedProject = dialogResult;
    }

    private async void DeleteProjectCommandExecute()
    {
        if ((bool)await ConfirmDialogProvider.ShowDialog(new ConfirmDialogViewModel(ConfirmDialogProvider)))
        {
            if (_currentProject != null && SelectedProject!.Id == _currentProject.Id)
                _currentProject = null;

            await _projectService.DeleteProject(SelectedProject!);
            GetData();
        }
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
}