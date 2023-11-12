using ITProcesses.Dialog;

namespace ITProcesses.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    #region Fields

    private BaseViewModel _currentChildView;
    private DialogProvider _dialogProvider = new DialogProvider();

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
    public DialogProvider DialogProvider
    {
        get => _dialogProvider;
        set
        {
            _dialogProvider = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    public MainWindowViewModel(bool skipAuth)
    {
        if(!skipAuth)
            ChangeView(new LoginViewModel(this));
        else
            ChangeView(new MainViewModel(this));
    }

    public void ChangeView(BaseViewModel selectedChildView)
    {
        CurrentChildView = selectedChildView;
    }
}