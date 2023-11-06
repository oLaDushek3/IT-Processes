using System;
using System.Collections.ObjectModel;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;

namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields
    
    private MainWindowViewModel _currentMainWindowViewModel;
    private BaseViewModel _currentChildView;

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
    public CommandHandler LogOutCommand => new(LogOutAsync);

    public CommandHandler ShowStatisticsViewCommand => new(OpenTasksListView);
    

    public MainViewModel(MainWindowViewModel currentMainViewModel)
    {
        _currentMainWindowViewModel = currentMainViewModel;
    }

    private async void LogOutAsync()
    {
        try
        {
            CurrentMainWindowViewModel.ChangeView(new LoginViewModel(CurrentMainWindowViewModel));
            SaveInfo.CreateAppSettingsDefault();
        }
        catch
        {
            
        }
    }
    
    public async void OpenTasksListView()
    {
        ChangeView(new TasksListViewModel(this));
    }
    
    public void ChangeView(BaseViewModel selectedView)
    {
        CurrentChildView = selectedView;
    }
}