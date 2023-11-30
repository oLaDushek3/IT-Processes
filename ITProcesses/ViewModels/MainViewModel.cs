using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields

    private ItprocessesContext _context = new();
    private IProjectService _projectService;
    private ITaskService _taskService;

    private MainWindowViewModel _currentMainWindowViewModel;
    private BaseViewModel? _currentChildView;

    private User _user;
    private Project _currentProject;

    private ObservableCollection<Tasks> _usersTaskList;

    #endregion

    #region Properties

    public MainWindowViewModel CurrentMainWindowViewModel
    {
        get => _currentMainWindowViewModel;
        set
        {
            _currentMainWindowViewModel = value;
            OnPropertyChanged();
        }
    }

    public BaseViewModel? CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged();
        }
    }

    public User User
    {
        get => _user;
        set
        {
            _user = value;
            OnPropertyChanged();
        }
    }

    public Project CurrentProject
    {
        get => _currentProject;
        set
        {
            _currentProject = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Tasks> UsersTaskList
    {
        get => _usersTaskList;
        set
        {
            _usersTaskList = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler OpenProjectDialogCommand => new(_ => OpenProjectDialog());

    public CommandHandler CreateTaskCommand => new(_ => CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
        new CreateTaskDialogViewModel(CurrentMainWindowViewModel.MainDialogProvider, this)));
    
    public CommandHandler LogOutCommand => new(_ => LogOutCommandExecute());

    public CommandHandler OpenTasksListCommand => new(_ => ChangeView(new TasksListViewModel(this)));

    public CommandHandler OpenTaskCommand =>
        new(selectedTask => ChangeView(new TaskViewModel((selectedTask as Tasks).Id, this)));

    //Constructor
    public MainViewModel(MainWindowViewModel currentMainViewModel, User user)
    {
        _projectService = new ProjectService(_context);
        _taskService = new TaskService(_context);
        _currentMainWindowViewModel = currentMainViewModel;
        _user = user;
        GetData();
    }

    //Methods
    public async void GetData()
    {
        try
        {
            _context = new();
            _projectService = new ProjectService(_context);
            _taskService = new TaskService(_context);
            
            CurrentProject = await _projectService.GetProjectById(Settings.CurrentProject);
            UsersTaskList = new ObservableCollection<Tasks>(await _taskService.GetTasksThisUser(_user.Id));
        }
        catch
        {
            OpenProjectDialog();
        }
    }

    private async void OpenProjectDialog()
    {
        var selectedProject = (Project?)await CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
            new ProjectDialogViewModel(CurrentMainWindowViewModel.MainDialogProvider, this, CurrentProject));

        if (selectedProject == null) return;

        CurrentProject = selectedProject;

        var settings = Settings;
        settings.UserName = Settings.UserName;
        settings.Password = Settings.Password;
        settings.CurrentProject = CurrentProject.Id;
        SaveInfo.SaveSettings(settings);

        ChangeView(null);
        GetData();
    }

    private async void LogOutCommandExecute()
    {
        if ((bool)await CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
                new ConfirmDialogViewModel(CurrentMainWindowViewModel.MainDialogProvider)))
        {
            CurrentMainWindowViewModel.ChangeView(new LoginViewModel(CurrentMainWindowViewModel));
            SaveInfo.CreateAppSettingsDefault();
        }
    }

    public void ChangeView(BaseViewModel? selectedView)
    {
        CurrentChildView = selectedView;
    }
}