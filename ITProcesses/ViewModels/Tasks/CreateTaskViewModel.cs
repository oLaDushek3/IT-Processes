using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class CreateTaskViewModel : BaseViewModel
{
    private readonly ItprocessesContext _context = new();
    private readonly ITaskService _taskService;
    
    private Tasks _tasks = new();
    
    public Tasks SelectedTask
    {
        get => _tasks;

        set
        {
            _tasks = value;
            OnPropertyChanged();
        }
    }

    public CreateTaskViewModel()
    {
        _taskService = new TaskService(_context);
    }
    
    public async void CreateNewTask()
    {
        var newTask = _taskService.CreateTask(_tasks);
    }
}