using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class EditTaskDialogViewModel : BaseViewModel
{
    #region Field

    private readonly ITaskService _taskService = new TaskService();
    private readonly DialogProvider _currentDialogProvider;

    private Tasks _editableTask = new();
    private List<TaskStatus> _statusList;

    #endregion

    #region Properties

    public Tasks EditableTask
    {
        get => _editableTask;
        set
        {
            _editableTask = value;
            OnPropertyChanged();
        }
    }

    public List<TaskStatus> StatusList
    {
        get => _statusList;

        set
        {
            _statusList = value;
            OnPropertyChanged();
        }
    }

    public DialogProvider ToolsDialogProvider { get; } = new();

    #endregion

    //Commands
    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    // public CommandHandler SaveCommand => new(_ => SaveCommandExecute(),
    //     _ => !string.IsNullOrEmpty(EditableProject.Name) && !string.IsNullOrEmpty(EditableProject.Description));
    public CommandHandler DeleteParticipantCommand => new(taskParticipants =>
        DeleteParticipantCommandExecute((taskParticipants as IList).Cast<UsersTask>().ToList()), 
        taskParticipants => taskParticipants != null && (taskParticipants as IList).Count != 0);

    public CommandHandler AddParticipantCommand => new(_ => AddParticipantCommandExecute());

    //Constructor
    public EditTaskDialogViewModel(Tasks editableTask, DialogProvider currentDialogProvider)
    {
        GetData(editableTask.Id);
        _currentDialogProvider = currentDialogProvider;
    }

    //Methods
    private async void GetData(Guid taskId)
    {
        EditableTask = await _taskService.GetTaskById(taskId);
        StatusList = await _taskService.GetAllStatuses();
    }

    private async void DeleteParticipantCommandExecute(List<UsersTask> taskParticipants)
    {
        foreach (UsersTask taskParticipant in taskParticipants)
        {
            EditableTask.UsersTasks.Remove(taskParticipant);
        }
    }

    private async void AddParticipantCommandExecute()
    {
        var selectedParticipants = (List<User>?)await ToolsDialogProvider.ShowDialog(
            new SelectionUserToTaskDialogViewModel(ToolsDialogProvider, EditableTask));

        if (selectedParticipants == null) return;

        foreach (var selectedParticipant in selectedParticipants)
        {
            EditableTask.UsersTasks.Add(new UsersTask
            {
                User = selectedParticipant,
                Task = EditableTask
            });
        }
    }
}