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
    
    public MainWindowViewModel()
    {
        ChangeView(new MainViewModel());
    }

    public void ChangeView(BaseViewModel selectedChildView)
    {
        CurrentChildView = selectedChildView;
    }
}