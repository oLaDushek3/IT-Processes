using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class EditProjectViewModel : BaseViewModel
{
    #region Field

    private readonly ProjectDialogViewModel _currentProjectDialogViewModel;
    private readonly IProjectService _projectService = new ProjectService();
    private readonly DialogProvider _currentDialogProvider;

    private Project _editableProject = new();

    #endregion

    #region Properties

    public Project EditableProject
    {
        get => _editableProject;
        set
        {
            _editableProject = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Commands
    public CommandHandler CancelCommand => new(_ =>  _currentDialogProvider.CloseDialog(null));

    public CommandHandler SaveCommand => new(_ => SaveCommandExecute(),
        _ => !string.IsNullOrEmpty(EditableProject.Name) && !string.IsNullOrEmpty(EditableProject.Description));

    //Constructor
    public EditProjectViewModel(ProjectDialogViewModel currentProjectDialogViewModel, Project selectedProject, DialogProvider currentDialogProvider)
    {
        _currentProjectDialogViewModel = currentProjectDialogViewModel;
        GetData(selectedProject.Id);
        _currentDialogProvider = currentDialogProvider;
    }

    //Methods
    private async void GetData(int projectId)
    {
        EditableProject = await _projectService.GetProjectById(projectId);
    }

    private async void SaveCommandExecute()
    {
        await _projectService.EditProject(EditableProject);
        
        _currentDialogProvider.CloseDialog(EditableProject);
    }
}