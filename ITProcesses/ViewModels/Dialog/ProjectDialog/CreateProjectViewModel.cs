using ITProcesses.Command;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class CreateProjectViewModel : BaseViewModel
{
    #region Field

    private readonly ProjectDialogViewModel _currentProjectDialogViewModel;
    private readonly IProjectService _projectService = new ProjectService();
    
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
    public CommandHandler CancelCommand => new (_ => _currentProjectDialogViewModel.ChangeView(null));
    public CommandHandler CreateCommand => new (_ => CreateCommandExecute(), _ => CreateCommandCanExecute());
    
    //Constructor
    public CreateProjectViewModel(ProjectDialogViewModel currentProjectDialogViewModel)
    {
        _currentProjectDialogViewModel = currentProjectDialogViewModel;
    }
    
    //Methods
    private async void CreateCommandExecute()
    {
        CreatedProject.UserId = _currentProjectDialogViewModel.CurrentMainViewModel.User.Id;
        await _projectService.CreateProject(CreatedProject);
        _currentProjectDialogViewModel.ChangeView(null);
        _currentProjectDialogViewModel.GetData();
    }
    
    private bool CreateCommandCanExecute()
    {
        return !string.IsNullOrEmpty(CreatedProject.Name) && !string.IsNullOrEmpty(CreatedProject.Description);
    }
}