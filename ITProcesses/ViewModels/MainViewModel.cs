using System;
using System.Collections.ObjectModel;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields

    private static readonly ItprocessesContext Context = new();
    private readonly IProjectService _projectService = new ProjectService(Context);
    
    private MainWindowViewModel _currentMainWindowViewModel;
    private BaseViewModel _currentChildView;
    
    private User _user;
    private Project _currentProject;

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

    public BaseViewModel CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler OpenProjectDialogCommand => new(_ => OpenProjectDialog());

    public CommandHandler LogOutCommand => new(_ => LogOutAsync());

    public CommandHandler OpenTasksListCommand => new(_ => OpenTasksList());

    //Constructor
    public MainViewModel(MainWindowViewModel currentMainViewModel, User user)
    {
        _currentMainWindowViewModel = currentMainViewModel;
        _user = user;
        GetData();
    }

    //Methods
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
    }

    private async void GetData()
    {
        try
        {
            CurrentProject = await _projectService.GetProjectById(Settings.CurrentProject);
        }
        catch
        {
            OpenProjectDialog();
        }
    }

    private async void LogOutAsync()
    {
        try
        {
            if ((bool)await CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
                    new ConfirmDialogViewModel(CurrentMainWindowViewModel.MainDialogProvider, "Вы уверены?")))
            {
                CurrentMainWindowViewModel.ChangeView(new LoginViewModel(CurrentMainWindowViewModel));
                SaveInfo.CreateAppSettingsDefault();
            }
        }
        catch
        {
        }
    }

    private void OpenTasksList()
    {
        ChangeView(new TasksListViewModel(this));
    }

    public void ChangeView(BaseViewModel selectedView)
    {
        CurrentChildView = selectedView;
    }
}