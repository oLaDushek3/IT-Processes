using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class CreateProjectViewModel : BaseViewModel
{
    #region Field

    private readonly ProjectDialogViewModel _currentProjectDialogViewModel;
    private readonly IProjectService _projectService = new ProjectService();
    private readonly DialogProvider _currentDialogProvider;
    
    private Project _createdProject = new();

    #endregion

    #region Properties
    
    public Project CreatedProject
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
    public CreateProjectViewModel(ProjectDialogViewModel currentProjectDialogViewModel, DialogProvider currentDialogProvider)
    {
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