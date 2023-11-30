using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class EditProjectDialogViewModel : BaseViewModel
{
    #region Field
    
    private readonly DialogProvider _currentDialogProvider;
    
    private readonly ItprocessesContext _context = new();
    private readonly IProjectService _projectService;

    private Project _editableProject;

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
        _ => EditableProject != null && !string.IsNullOrEmpty(EditableProject.Name) && !string.IsNullOrEmpty(EditableProject.Description));

    //Constructor
    public EditProjectDialogViewModel(Project selectedProject, DialogProvider currentDialogProvider)
    {
        _projectService = new ProjectService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData(selectedProject.Id);
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