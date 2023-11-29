using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{
    #region Fields

    private readonly MainViewModel _currentMainViewModel;
    private readonly ITaskService _taskService;
    private readonly DialogProvider _currentDialogProvider;
    
    private Tasks _currentTask;

    #endregion

    #region Properties

    public Tasks SelectedTask
    {
        get => _currentTask;

        set
        {
            _currentTask = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    //Commands
    public CommandHandler CancelCommand => new(_ => _currentMainViewModel.ChangeView(new TasksListViewModel(_currentMainViewModel)));
    public CommandHandler EditTaskCommand => new(_ => EditTaskCommandExecute());
    public CommandHandler DeleteTaskCommand => new(_ => DeleteTaskCommandExecute());
    
    //Constructor
    public TaskViewModel(Guid selectedTaskGuid, MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService();
        _currentMainViewModel = currentMainViewModel;
        _currentDialogProvider = currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider;
        GetData(selectedTaskGuid);
    }

    //Methods
    private async void GetData(Guid selectedTaskGuid)
    {
        SelectedTask = await _taskService.GetTaskById(selectedTaskGuid);
    }
    
    private async void EditTaskCommandExecute()
    {
        var dialogResult =
            (Tasks?)await _currentDialogProvider.ShowDialog(
                new EditTaskDialogViewModel(_currentTask, _currentDialogProvider));

        if (dialogResult == null) return;

        SelectedTask = dialogResult;
    }

    private async void DeleteTaskCommandExecute()
    {
        if ((bool)await _currentDialogProvider.ShowDialog(new ConfirmDialogViewModel(_currentDialogProvider,
                "Вы уверены?")))
        {
            await _taskService.DeleteTask(_currentTask);
            _currentMainViewModel.ChangeView(new TasksListViewModel(_currentMainViewModel));
        }
    }
}