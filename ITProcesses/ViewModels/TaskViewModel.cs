using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{
    #region Fields

    private readonly ITaskService _taskService;
    
    private Tasks _selectedTask;
    private ObservableCollection<TaskStatus> _statusList;


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
    public TaskViewModel(Guid selectedTaskGuid)
    {
        _taskService = new TaskService();
        
        GetData(selectedTaskGuid);
    }

    private async void GetData(Guid selectedTaskGuid)
    {
        StatusList = new ObservableCollection<TaskStatus>(await _taskService.GetAllStatuses());
        SelectedTask = (await _taskService.GetTaskById(selectedTaskGuid));
    }
}