using System;
using System.Windows.Input;

namespace ITProcesses.ViewModels;

public class CommandViewModel : ICommand
{
    //Fields
    private readonly Action<Object> _executeAction;
    private readonly Predicate<Object> _canExecuteAction;

    //Constructors
    public CommandViewModel(Action<object> executeAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = null;
    }

    public CommandViewModel(Action<object> executeAction, Predicate<object> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = canExecuteAction;
    }

    //Events
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    //Methods
    public bool CanExecute(object parameter)
    {
        return _canExecuteAction == null || _canExecuteAction(parameter);
    }

    public void Execute(object parameter)
    {
        _executeAction(parameter);
    }
}