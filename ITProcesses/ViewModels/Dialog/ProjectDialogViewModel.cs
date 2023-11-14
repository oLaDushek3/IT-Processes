using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Automation;
using System.Windows.Input;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class ProjectDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _dialogProvider;
    private readonly ITaskService _taskService = new TaskService();

    private ObservableCollection<Project> _projectsList = null!;
    private Project? _selectedProject;

    #endregion

    #region Properties
    
    public ObservableCollection<Project> ProjectsList
    {
        get => _projectsList;
        set
        {
            _projectsList = value;
            OnPropertyChanged();
        }
    }
    public Project SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    //Commands
    public CommandHandler<Project> AcceptSelectCommand => new (AcceptSelectExecute, AcceptSelectCanExecute);
    
    //Constructor
    public ProjectDialogViewModel(DialogProvider dialogProvider)
    {
        _dialogProvider = dialogProvider;
        GetData();
    }
    
    //Methods
    private async void GetData()
    {
        ProjectsList = new ObservableCollection<Project>(await _taskService.GetAllProject());
    }
    
    private void AcceptSelectExecute(Project selectedProject)
    {
        _dialogProvider.CloseDialog(SelectedProject);
    }

    private bool AcceptSelectCanExecute(Project? selectedProject)
    {
        return _selectedProject != null;
    }

    // public RelayCommand Command => new(AcceptSelectCanExecute, AcceptSelectExecute);
    //
    // public class RelayCommand : ICommand
    // {
    //     private readonly Predicate<object> _canExecute;
    //     private readonly Action<object> _execute;
    //
    //     public RelayCommand(Predicate<object> canExecute, Action<object> execute)
    //     {
    //         _canExecute = canExecute;
    //         _execute = execute;
    //     }
    //
    //     public event EventHandler CanExecuteChanged
    //     {
    //         add => CommandManager.RequerySuggested += value;
    //         remove => CommandManager.RequerySuggested -= value;
    //     }
    //
    //     public bool CanExecute(object parameter)
    //     {
    //         return _canExecute(parameter);
    //     }
    //
    //     public void Execute(object parameter)
    //     {
    //         _execute(parameter);
    //     }
    // }
}