using ITProcesses.Command;
using ITProcesses.Dialog;

namespace ITProcesses.ViewModels;

public class ErrorDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;
    private string _errorMessage;

    #endregion

    #region Properties

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler OkCommand => new(_ => Confirm(false));
    
    //Constructor
    public ErrorDialogViewModel(DialogProvider dialogProvider, string message)
    {
        _currentDialogProvider = dialogProvider;
        ErrorMessage = message;
    }
    
    //Methods
    private void Confirm(bool decision)
    {
        _currentDialogProvider.CloseDialog(decision);
    }
}