using ITProcesses.Command;
using ITProcesses.Dialog;

namespace ITProcesses.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;
    private string _message;

    #endregion

    #region Properties

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler ConfirmCommand => new(obj => Confirm(obj as bool? ?? false));
    
    //Constructor
    public ConfirmDialogViewModel(DialogProvider dialogProvider, string message)
    {
        _currentDialogProvider = dialogProvider;
        Message = message;
    }
    
    //Methods
    private void Confirm(bool decision)
    {
        _currentDialogProvider.CloseDialog(decision);
    }
}