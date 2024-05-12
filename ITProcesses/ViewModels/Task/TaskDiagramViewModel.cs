using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private async void GetData()
    {
        _selectedUser = await _userService.GetUserById(_user.Id);
        var allTasks = await _taskService.GetTasksThisUser(_user.Id);
        var allTasksCount = allTasks.Count;
        Count = $"Сотрудник учавствует в {allTasksCount} задачах";
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