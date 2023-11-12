using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{
    #region Fields

    private readonly ITaskService _taskService;
    
    private Tasks _selectedTask;
    private ObservableCollection<TaskStatus> _statusList;
    private MainViewModel _currentMainViewModel;



    #endregion

    #region Properties

    public Tasks SelectedTask
    {
        get => _selectedTask;

        set
        {
            _selectedTask = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<TaskStatus> StatusList
    {
        get => _statusList;

        set
        {
            _statusList = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    
    //Constructor
    public TaskViewModel(Guid selectedTaskGuid, MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService();
        _currentMainViewModel = currentMainViewModel;
        GetData(selectedTaskGuid);
    }

    public CommandHandler BackCommand => new(BackTaskListView);

    private async void GetData(Guid selectedTaskGuid)
    {
        StatusList = new ObservableCollection<TaskStatus>(await _taskService.GetAllStatuses());
        SelectedTask = (await _taskService.GetTaskById(selectedTaskGuid));
    }

    private void BackTaskListView()
    { 
        _currentMainViewModel.ChangeView(new TasksListViewModel(_currentMainViewModel));
    }

    private void EditTask()
    {
       // _taskService.
    }
    
}