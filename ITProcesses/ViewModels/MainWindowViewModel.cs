using ITProcesses.Dialog;
using ITProcesses.Models;

namespace ITProcesses.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    #region Fields

    private BaseViewModel _currentChildView;

    #endregion

    #region Properties

    public BaseViewModel CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged();
        }
    }
    public DialogProvider MainDialogProvider { get; } = new();

    #endregion
    
    public MainWindowViewModel(User? user)
    {
        if(user == null)
            ChangeView(new LoginViewModel(this));
        else
            ChangeView(new MainViewModel(this, user));
    }

    public void ChangeView(BaseViewModel selectedChildView)
    {
        CurrentChildView = selectedChildView;
    }
}