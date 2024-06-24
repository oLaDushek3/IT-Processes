using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Models;
using ITProcesses.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace ITProcesses.ViewModels;

public class TaskDiagramViewModel : BaseViewModel
{
    private readonly ItprocessesContext _context = new();
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;

    private List<Role> _roleList;
    private List<User> _allUserList;
    private ObservableCollection<User> _displayedUserList;
    private SeriesCollection _selectedUserTasks = new();
    private User? _selectedUser;
    private User? _user;
    private string count = string.Empty;
    private string endedTask = String.Empty;
    private string workTask = String.Empty;

    public TaskDiagramViewModel(User user)
    {
        _userService = new UserService(_context);
        _taskService = new TaskService(_context);
        _user = user;
        GetData();
    }

    // public User? SelectedUser
    // {
    //     get => _selectedUser;
    //     set
    //     {
    //         _selectedUser = value;
    //         OnPropertyChanged();
    //         foreach (var usersTask in _selectedUser.UsersTasks)
    //         {
    //             SelectedUserTasks.Add(
    //                 new PieSeries
    //                 {
    //                     Title = usersTask.Task.Name,
    //                     Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
    //                     DataLabels = true
    //                 });
    //         }
    //
    //         SelectedUserTasks = new SeriesCollection(_selectedUser.UsersTasks);
    //     }
    // }

    public LiveCharts.SeriesCollection SelectedUserTasks
    {
        get => _selectedUserTasks;
        set
        {
            _selectedUserTasks = value;
            OnPropertyChanged();
        }
    }

    public string Count
    {
        get => count;
        set
        {
            count = value;
            OnPropertyChanged();
        }
    }

    public string EndedTask
    {
        get => endedTask;
        set
        {
            endedTask = value;
            OnPropertyChanged();
        }
    }

    public string WorkTask
    {
        get => workTask;
        set
        {
            workTask = value;
            OnPropertyChanged();
        }
    }

    private async void GetData()
    {
        ItprocessesContext context = new ItprocessesContext();
        _selectedUser = await _userService.GetUserById(_user.Id);
        var allTasks = await _taskService.GetTasksThisUser(_user.Id);
        var allTasksCount = allTasks.Count;
        EndedTask = $"Выполненных задач: {allTasks.Count(a => a.StatusId == 3)}";
        Count = $"Сотрудник учавствует в {allTasksCount} задачах";
        WorkTask = $"Задач в работе: {allTasks.Count(a => a.StatusId == 2)}";
        OnPropertyChanged();
        _allUserList = await _userService.GetAllUsers();
        foreach (var usersTask in _selectedUser.UsersTasks)
        {
            SelectedUserTasks.Add(
                new PieSeries
                {
                    Title = usersTask.Task.Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(_selectedUser.UsersTasks.Count) },
                    DataLabels = true
                });
        }
// SelectedUserTasks = new SeriesCollection(_selectedUser.UsersTasks);
    }
}