using ITProcesses.Command;
using ITProcesses.Dialog;

namespace ITProcesses.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;

    #endregion

    //Commands
    public CommandHandler ConfirmCommand => new(obj => Confirm(obj as bool? ?? false));
    
    //Constructor
    public ConfirmDialogViewModel(DialogProvider dialogProvider)
    {
        _currentDialogProvider = dialogProvider;
    }
    
    //Methods
    private void Confirm(bool decision)
    {
        _currentDialogProvider.CloseDialog(decision);
    }
}