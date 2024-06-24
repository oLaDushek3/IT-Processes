using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.FilesGenerate;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.ViewModels.ChatDialog;
using ModernWpf.Controls;

namespace ITProcesses.ViewModels;

public class TaskViewModel : BaseViewModel
{
    #region Fields

    private readonly ItprocessesContext _context = new();

    private MainViewModel _currentMainViewModel;
    private readonly ITaskService _taskService;
    private readonly DialogProvider _currentDialogProvider;
    private User _user;
    private readonly DocxGenerate _docxGenerate = new DocxGenerate();
    private readonly XlsxGenerate _xlsxGenerate = new XlsxGenerate();

    private Tasks _currentTask;

    #endregion

    #region Properties

    public MainViewModel CurrentMainViewModel
    {
        get => _currentMainViewModel;
        private set
        {
            _currentMainViewModel = value;
            OnPropertyChanged();
        }
    }

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
    public CommandHandler CancelCommand =>
        new(_ => _currentMainViewModel.ChangeView(new TasksListViewModel(_currentMainViewModel)));

    public CommandHandler EditTaskCommand => new(_ => EditTaskCommandExecute());
    public CommandHandler DeleteTaskCommand => new(_ => DeleteTaskCommandExecute());

    public CommandHandler OpenChatCommand => new(_ => ChatCommandExecute());

    public CommandHandler OpenDocumentCommand => new(selectedDocument =>
        OpenDocumentCommandExecute((selectedDocument as TaskDocument).Documents));

    public CommandHandler DocXGenerateCommand => new(_ => DocXGenerateExecute());

    public CommandHandler XlsXGenerateCommand => new(_ => XlsXGenerateExecute());

    //Constructor
    public TaskViewModel(Guid selectedTaskGuid, MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService(_context);

        CurrentMainViewModel = currentMainViewModel;
        _user = currentMainViewModel.User;
        _currentDialogProvider = currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider;
        GetData(selectedTaskGuid);
    }

    //Methods
    private async void GetData(Guid selectedTaskGuid)
    {
        SelectedTask = await _taskService.GetTaskById(selectedTaskGuid);
    }

    private void OpenDocumentCommandExecute(Document selectedDocument)
    {
        Process process = new();
        process.StartInfo = new ProcessStartInfo(selectedDocument.Path)
        {
            UseShellExecute = true
        };
        process.Start();
    }

    private async void EditTaskCommandExecute()
    {
        var dialogResult =
            (Tasks?)await _currentDialogProvider.ShowDialog(
                new EditTaskDialogViewModel(_currentTask, _currentDialogProvider));

        if (dialogResult == null) return;

        SelectedTask = dialogResult;

        _currentMainViewModel.GetData();
    }

    private async void DeleteTaskCommandExecute()
    {
        if ((bool)await _currentDialogProvider.ShowDialog(new ConfirmDialogViewModel(_currentDialogProvider)))
        {
          await   _taskService.DeleteTask(_currentTask);
            _currentMainViewModel.GetData();
            _currentMainViewModel.ChangeView(new TasksListViewModel(_currentMainViewModel));
        }
    }

    private async void ChatCommandExecute()
    {
        ContentDialog contentDialog = new ContentDialog
        {
            Title = "Чат",
            Content = new Views.Task.ChatDialog.ChatDialog(),
            CloseButtonText = "Закрыть чат",
            DataContext = new ChatDialogViewModel(_currentTask.Id, _user)
        };
        await contentDialog.ShowAsync();
    }

    private void DocXGenerateExecute()
    {
        _docxGenerate.GeneratePeopleFromTask(_currentTask);
    }

    private async void XlsXGenerateExecute()
    {
        
       _xlsxGenerate.GenerateExcel(_currentTask.UsersTasks.ToList());
    }
}