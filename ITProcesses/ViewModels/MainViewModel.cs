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

    private MainWindowViewModel _currentMainWindowViewModel;

    private User _user;
    private BaseViewModel _currentChildView;

    private ITaskService _taskService = new TaskService();
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
        var selectedProject = (Project?)await CurrentMainWindowViewModel.DialogProvider.ShowDialog(
            new ProjectDialogViewModel(CurrentMainWindowViewModel.DialogProvider, this));

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
        if (Settings.CurrentProject != 0)
            CurrentProject = await _taskService.GetProjectById(Settings.CurrentProject);
        else
            OpenProjectDialog();
    }

    private async void LogOutAsync()
    {
        try
        {
            if ((bool)await CurrentMainWindowViewModel.DialogProvider.ShowDialog(
                    new ConfirmDialogViewModel(CurrentMainWindowViewModel.DialogProvider, "Вы уверены?")))
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