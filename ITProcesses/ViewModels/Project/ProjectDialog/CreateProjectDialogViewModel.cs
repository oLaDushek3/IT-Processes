using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class CreateProjectDialogViewModel : BaseViewModel
{
    #region Field
    
    private readonly ProjectDialogViewModel _currentProjectDialogViewModel;
    private readonly DialogProvider _currentDialogProvider;
    
    private readonly ItprocessesContext _context = new();
    private readonly IProjectService _projectService;
    
    private Project _createdProject = new();

    #endregion

    #region Properties
    
    public Models.Project CreatedProject
    {
        get => _createdProject;
        set
        {
            _createdProject = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    //Commands
    public CommandHandler CancelCommand => new (_ =>  _currentDialogProvider.CloseDialog(null));
    public CommandHandler CreateCommand => new (_ => CreateCommandExecute(), _ => !string.IsNullOrEmpty(CreatedProject.Name) && !string.IsNullOrEmpty(CreatedProject.Description));
    
    //Constructor
    public CreateProjectDialogViewModel(ProjectDialogViewModel currentProjectDialogViewModel, DialogProvider currentDialogProvider)
    {
        _projectService = new ProjectService(_context);
        _currentProjectDialogViewModel = currentProjectDialogViewModel;
        _currentDialogProvider = currentDialogProvider;
    }
    
    //Methods
    private async void CreateCommandExecute()
    {
        CreatedProject.UserId = _currentProjectDialogViewModel.CurrentMainViewModel.User.Id;
        await _projectService.CreateProject(CreatedProject);
        
        _currentDialogProvider.CloseDialog(CreatedProject);
    }
}