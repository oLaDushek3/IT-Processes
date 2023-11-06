using System;
using System.Collections.ObjectModel;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;

namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields
    
    private MainWindowViewModel _currentMainViewModel;
    private BaseViewModel _currentChildView;

    #endregion
    
    #region Properties

    public MainWindowViewModel CurrentMainViewModel
    {
        get => _currentMainViewModel;
        set
        {
            _currentMainViewModel = value;
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

    public CommandHandler ShowStatisticsViewCommand => new(ChangeView);
    

    public MainViewModel(MainWindowViewModel currentMainViewModel)
    {
        _currentMainViewModel = currentMainViewModel;
    }

    private async void LogOutAsync()
    {
        try
        {
            CurrentMainViewModel.ChangeView(new LoginViewModel(CurrentMainViewModel));
            SaveInfo.CreateAppSettingsDefault();
        }
        catch
        {
            
        }
    }
    public async void ChangeView()
    {
        CurrentChildView = new TasksListViewModel();
    }
}