using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using Microsoft.Win32;

namespace ITProcesses.ViewModels;

public class EditTaskDialogViewModel : BaseViewModel
{
    #region Field

    private readonly ItprocessesContext _context = new();

    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly DialogProvider _currentDialogProvider;

    private Tasks _editableTask = new();
    private List<TaskStatus> _statusList;

    private readonly List<UsersTask> _participantDeletionList = new();
    private readonly List<TaskDocument> _documentDeletionList = new();

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

    public CommandHandler SaveCommand => new(_ => SaveCommandExecute());

    public CommandHandler AddDocumentsCommand => new(_ => AddDocumentsCommandExecute());

    public CommandHandler DeleteDocumentsCommand => new(taskDocument =>
            DeleteDocumentsCommandExecute((taskDocument as IList).Cast<TaskDocument>().ToList()),
        taskDocument => taskDocument != null && (taskDocument as IList).Count != 0);

    public CommandHandler AddParticipantsCommand => new(_ => AddParticipantsCommandExecute());

    public CommandHandler DeleteParticipantsCommand => new(taskParticipants =>
            DeleteParticipantsCommandExecute((taskParticipants as IList).Cast<UsersTask>().ToList()),
        taskParticipants => taskParticipants != null && (taskParticipants as IList).Count != 0);

    //Constructor
    public EditTaskDialogViewModel(Tasks editableTask, DialogProvider currentDialogProvider)
    {
        _taskService = new TaskService(_context);
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData(editableTask.Id);
    }

    //Methods
    private async void GetData(Guid taskId)
    {
        EditableTask = await _taskService.GetTaskById(taskId);
        StatusList = await _taskService.GetAllStatuses();
    }

    private async void SaveCommandExecute()
    {
        foreach (var participant in _participantDeletionList)
            await _taskService.DeleteUsersTask(participant);
        
        foreach (var document in _documentDeletionList)
            await _taskService.DeleteTaskDocument(document);

        await _taskService.UpdateTask(EditableTask);
        _currentDialogProvider.CloseDialog(EditableTask);
    }

    private void AddDocumentsCommandExecute()
    {
        OpenFileDialog fileDialog = new OpenFileDialog
        {
            Multiselect = true,
            Title = "Выберите файлы",
            InitialDirectory = @"C:\"
        };

        if ((bool)fileDialog.ShowDialog()!)
        {
            foreach (string file in fileDialog.FileNames)
            {
                EditableTask.TaskDocuments.Add(new TaskDocument
                {
                    Documents = new Document
                    {
                        Id = Guid.NewGuid(),
                        Name = Path.GetFileName(file),
                        Path = file
                    },
                    Task = EditableTask
                });
            }
        }
    }

    private void DeleteDocumentsCommandExecute(List<TaskDocument> taskDocuments)
    {
        foreach (TaskDocument taskDocument in taskDocuments)
        {
            EditableTask.TaskDocuments.Remove(taskDocument);
            _documentDeletionList.Add(taskDocument);
        }
    }

    private async void AddParticipantsCommandExecute()
    {
        var selectedParticipants = (List<User>?)await ToolsDialogProvider.ShowDialog(
            new SelectionUserToTaskDialogViewModel(ToolsDialogProvider, EditableTask.Id, _taskService));

        if (selectedParticipants == null) return;

        foreach (var selectedParticipant in selectedParticipants)
        {
            UsersTask newUsersTask = new()
            {
                User = await _userService.GetUserById(selectedParticipant.Id),
                Task = EditableTask
            };
            EditableTask.UsersTasks.Add(newUsersTask);
        }
    }

    private void DeleteParticipantsCommandExecute(List<UsersTask> taskParticipants)
    {
        foreach (var taskParticipant in taskParticipants)
        {
            EditableTask.UsersTasks.Remove(taskParticipant);
            _participantDeletionList.Add(taskParticipant);
        }
    }
}