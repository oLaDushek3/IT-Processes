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
            OnPropertyChanged(nameof(CurrentChildView));
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