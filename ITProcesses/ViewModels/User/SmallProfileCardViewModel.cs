using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;

namespace ITProcesses.ViewModels;

public class SmallProfileCardViewModel : BaseViewModel
{
    #region Fields

    private User _profileUser;
    private readonly DialogProvider _currentDialogProvider;
    private readonly MainWindowViewModel _currentMainWindowViewModel;

    #endregion

    #region Properties

    public User ProfileUser
    {
        get => _profileUser;
        set
        {
            _profileUser = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    //Commands
    public CommandHandler OpenProfileCommand => new(_ => _currentDialogProvider.ShowDialog(new ProfileCardDialogViewMode(ProfileUser, _currentDialogProvider, _currentMainWindowViewModel)));
    
    //Constructor
    public SmallProfileCardViewModel(User profileUser, DialogProvider currentDialogProvider, MainWindowViewModel currentMainWindowViewModel)
    {
        ProfileUser = profileUser;
        _currentDialogProvider = currentDialogProvider;
        _currentMainWindowViewModel = currentMainWindowViewModel;
    }
}