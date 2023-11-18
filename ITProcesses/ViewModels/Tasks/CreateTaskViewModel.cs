using ITProcesses.Models;
using ITProcesses.Services;

namespace ITProcesses.ViewModels;

public class CreateTaskViewModel:BaseViewModel
{
    private readonly ITaskService _taskService;
    private Tasks _tasks;

    public CreateTaskViewModel()
    {
        _taskService = new TaskService();
        _tasks = new Tasks();
    }
    
    public Tasks SelectedTask
    {
        get => _tasks;

        set
        {
            _tasks = value;
            OnPropertyChanged();
        }
    }

    public async void CreateNewTask()
    {
        var newTask = _taskService.CreateTask(_tasks);
    }
}