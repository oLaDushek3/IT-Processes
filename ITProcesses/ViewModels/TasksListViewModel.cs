using System.Collections.ObjectModel;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.Views;

namespace ITProcesses.ViewModels;

public class TasksListViewModel : BaseViewModel
{
    #region Fields

    private ITaskService _taskService;
    private MainViewModel _currentMainViewModel;

    private ObservableCollection<Tasks> _tasksList;
    private Tasks _selectedTask;

    #endregion


    #region Properties

    public ObservableCollection<Tasks> TasksList
    {
        get => _tasksList;
        set
        {
            _tasksList = value;
            OnPropertyChanged();
        }
    }

    public Tasks SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged();

            if (value != null)
                OpenTask();
        }
    }

    #endregion

    //Constructor
    public TasksListViewModel(MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService();
        _currentMainViewModel = currentMainViewModel;

        GetData();
    }

    //Methods
    private async void GetData()
    {
        TasksList = new ObservableCollection<Tasks>(await _taskService.GetAllTask());
    }

    private void OpenTask()
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(SelectedTask.Id, _currentMainViewModel));
    }
}