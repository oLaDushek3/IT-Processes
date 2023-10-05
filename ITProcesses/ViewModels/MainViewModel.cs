namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
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
   
    
    public MainViewModel()
    {
        _currentChildView = new LoginViewModel();
        CurrentChildView = _currentChildView;
    }
}