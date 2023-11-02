using System;
using ITProcesses.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{

    #region Fields

    private  Tasks _selectedTask;

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

    #endregion
    
    
    //Constructor
    public TaskViewModel()
    {
        //SelectedTask = selectedTask;
    }
}