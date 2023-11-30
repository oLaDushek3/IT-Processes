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
using Type = ITProcesses.Models.Type;

namespace ITProcesses.ViewModels;

public class CreateTaskDialogViewModel : BaseViewModel
{
    #region Field

    private readonly ItprocessesContext _context = new();

    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly DialogProvider _currentDialogProvider;
    private readonly MainViewModel _currentMainViewModel;

    private Tasks _createdTask = new();
    private List<Type> _typeList;

    #endregion

    #region Properties

    public Tasks CreatedTask
    {
        get => _createdTask;
        set
        {
            _createdTask = value;
            OnPropertyChanged();
        }
    }

    public List<Type> TypeList
    {
        get => _typeList;

        set
        {
            _typeList = value;
            OnPropertyChanged();
        }
    }

    public DialogProvider ToolsDialogProvider { get; } = new();

    #endregion

    //Commands
    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler CreateCommand => new(_ => CreateCommandExecute());

    public CommandHandler SelectBeforeTaskCommand => new(_ => SelectBeforeTaskCommandExecute());

    public CommandHandler RemoveBeforeTaskCommand => new(_ =>
    {
        CreatedTask.BeforeTask = null;
        OnPropertyChanged("CreatedTask");
    });
    
    public CommandHandler AddDocumentsCommand => new(_ => AddDocumentsCommandExecute());

    public CommandHandler DeleteDocumentsCommand => new(taskDocument =>
            DeleteDocumentsCommandExecute((taskDocument as IList).Cast<TaskDocument>().ToList()),
        taskDocument => taskDocument != null && (taskDocument as IList).Count != 0);

    public CommandHandler AddParticipantsCommand => new(_ => AddParticipantsCommandExecute());

    public CommandHandler DeleteParticipantsCommand => new(taskParticipants =>
            DeleteParticipantsCommandExecute((taskParticipants as IList).Cast<UsersTask>().ToList()),
        taskParticipants => taskParticipants != null && (taskParticipants as IList).Count != 0);

    //Constructor
    public CreateTaskDialogViewModel(DialogProvider currentDialogProvider, MainViewModel currentMainViewModel)
    {
        _currentMainViewModel = currentMainViewModel;
        _taskService = new TaskService(_context);
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        TypeList = await _taskService.GetAllTypes();
    }

    private async void CreateCommandExecute()
    {
        try
        {
            CreatedTask.DateCreateTimestamp = CreatedTask.DateCreateTimestamp.ToUniversalTime();
            CreatedTask.DateStartTimestamp = CreatedTask.DateCreateTimestamp.ToUniversalTime();
            CreatedTask.DateEndTimestamp = CreatedTask.DateCreateTimestamp.ToUniversalTime();
            CreatedTask.Status = (await _taskService.GetAllStatuses()).FirstOrDefault();
            CreatedTask.ProjectId = _currentMainViewModel.CurrentProject.Id;

            await _taskService.CreateTask(CreatedTask);
            _currentDialogProvider.CloseDialog(CreatedTask);
        }
        catch (Exception e)
        {
            ToolsDialogProvider.ShowDialog(new ErrorDialogViewModel(ToolsDialogProvider,
                "Не все поля заполнены"));
        }
    }

    private async void SelectBeforeTaskCommandExecute()
    {
        var selectedTask = (Tasks?)await ToolsDialogProvider.ShowDialog(
            new SelectionBeforeTaskDialogViewModel(ToolsDialogProvider));

        if (selectedTask == null) return;

        CreatedTask.BeforeTaskNavigation = await _taskService.GetTaskById(selectedTask.Id);
        OnPropertyChanged("CreatedTask");
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
                CreatedTask.TaskDocuments.Add(new TaskDocument
                {
                    Documents = new Document
                    {
                        Id = Guid.NewGuid(),
                        Name = Path.GetFileName(file),
                        Path = file
                    },
                    Task = CreatedTask
                });
            }
        }
    }

    private void DeleteDocumentsCommandExecute(List<TaskDocument> taskDocuments)
    {
        foreach (TaskDocument taskDocument in taskDocuments)
        {
            CreatedTask.TaskDocuments.Remove(taskDocument);
        }
    }

    private async void AddParticipantsCommandExecute()
    {
        var selectedParticipants = (List<User>?)await ToolsDialogProvider.ShowDialog(
            new SelectionUserToTaskDialogViewModel(ToolsDialogProvider, CreatedTask.Id));

        if (selectedParticipants == null) return;

        foreach (var selectedParticipant in selectedParticipants)
        {
            UsersTask newUsersTask = new()
            {
                User = await _userService.GetUserById(selectedParticipant.Id),
                Task = CreatedTask
            };
            CreatedTask.UsersTasks.Add(newUsersTask);
        }
    }

    private void DeleteParticipantsCommandExecute(List<UsersTask> taskParticipants)
    {
        foreach (var taskParticipant in taskParticipants)
        {
            CreatedTask.UsersTasks.Remove(taskParticipant);
        }
    }
}