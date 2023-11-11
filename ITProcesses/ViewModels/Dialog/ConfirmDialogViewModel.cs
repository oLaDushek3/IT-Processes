using ITProcesses.Command;
using ITProcesses.Dialog;

namespace ITProcesses.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    #region Field

    private DialogProvider _dialogProvider;
    private string _message;

    #endregion

    #region Properties

    public DialogProvider DialogProvider
    {
        get => _dialogProvider;
        set
        {
            _dialogProvider = value;
            OnPropertyChanged();
        }
    }
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
    public CommandHandler ClickYesCommand => new(ClickYes);
    public CommandHandler ClickNoCommand => new(ClickNo);
    
    //Constructor
    public ConfirmDialogViewModel(DialogProvider dialogProvider, string message)
    {
        DialogProvider = dialogProvider;
        Message = message;
    }
    
    //Methods
    private void ClickYes()
    {
        DialogProvider.CloseDialog(true);
    }
    private void ClickNo()
    {
        DialogProvider.CloseDialog(false);
    }
}