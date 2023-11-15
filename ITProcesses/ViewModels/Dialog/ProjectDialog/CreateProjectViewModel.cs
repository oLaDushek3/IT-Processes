using ITProcesses.Command;

namespace ITProcesses.ViewModels;

public class CreateProjectViewModel : BaseViewModel
{
    #region Field

    private readonly ProjectDialogViewModel _currentProjectDialogViewModel;

    #endregion

    #region Properties
    
    /*public BaseViewModel? CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged();
        }
    }*/
    
    #endregion
    
    //Commands
    public CommandHandler CancelCommand => new (_ => _currentProjectDialogViewModel.ChangeView(null));
    
    //Constructor
    public CreateProjectViewModel(ProjectDialogViewModel currentProjectDialogViewModel)
    {
        _currentProjectDialogViewModel = currentProjectDialogViewModel;
    }
}