using System;
using ITProcesses.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{

    #region Fields

    private  Tasks _selectedTask;
    private  DateTime _testDateTime;

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
    public DateTime TestDateTime
    {
        get => _testDateTime;

        set
        {
            _testDateTime = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    
    //Constructor
    public TaskViewModel(Tasks selectedTask)
    {
        SelectedTask = selectedTask;
    }
}