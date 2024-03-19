using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace ITProcesses.ViewModels;

public class UserDialogViewModel : BaseViewModel
{
    #region Field

    public readonly MainViewModel CurrentMainViewModel;
    private readonly DialogProvider _currentDialogProvider;

    private readonly ItprocessesContext _context = new();
    private readonly IUserService _userService;

    private List<Role> _roleList;
    private List<User> _allUserList;
    private ObservableCollection<User> _displayedUserList;
    private SeriesCollection  _selectedUserTasks = new();
    private User? _selectedUser;

    //Search and sort
    private string _searchString;
    private List<User>? _searchUserList;

    private Role? _selectedSortRole;
    private List<User>? _sortUserList;

    #endregion

    #region Properties

    public DialogProvider ConfirmDialogProvider { get; } = new();

    public DialogProvider CreateEditDialogProvider { get; } = new();

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
            foreach (var usersTask in _selectedUser.UsersTasks)
            {
                SelectedUserTasks.Add(           
                    new PieSeries
                {
                    Title = usersTask.Task.Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                });
            }
            SelectedUserTasks = new SeriesCollection(_selectedUser.UsersTasks);
        }
    }

    public List<Role> RoleList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<User> DisplayedUserList
    {
        get => _displayedUserList;
        set
        {
            _displayedUserList = value;
            OnPropertyChanged();
        }
    }
    public LiveCharts.SeriesCollection SelectedUserTasks
    {
        get => _selectedUserTasks;
        set
        {
            _selectedUserTasks = value;
            OnPropertyChanged();
        }
    }

    //Search and sort
    public string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            OnPropertyChanged();
            if (value == "")
                _searchUserList = null;
            SearchUsers();
        }
    }

    public Role? SelectedSortRole
    {
        get => _selectedSortRole;
        set
        {
            _selectedSortRole = value;
            OnPropertyChanged();
            SortingUsersByRole();
        }
    }

    #endregion

    //Commands
    public CommandHandler CancelCommand =>
        new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler ClearSortByRoleCommand => new(_ => SelectedSortRole = null);

    public CommandHandler CreateUserCommand => new(_ => CreateUserCommandExecute());

    public CommandHandler EditUserCommand =>
        new(_ => EditUserCommandExecute(), _ => SelectedUser != null);

    public CommandHandler DeleteUserCommand =>
        new(_ => DeleteUserCommandExecute(), _ => SelectedUser != null);

    //Constructor
    public UserDialogViewModel(DialogProvider currentDialogProvider, MainViewModel currentMainViewModel)
    {
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        CurrentMainViewModel = currentMainViewModel;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        _allUserList = await _userService.GetAllUsers();
        DisplayedUserList = new ObservableCollection<User>(_allUserList);

        RoleList = await _userService.GetAllRoles();
    }

    private async void CreateUserCommandExecute()
    {
        var dialogResult =
            (User?)await CreateEditDialogProvider.ShowDialog(new CreateUserDialogViewModel(ConfirmDialogProvider, CreateEditDialogProvider));

        if (dialogResult == null) return;

        _allUserList.Add(dialogResult);
        DisplayedUserList = new ObservableCollection<User>(_allUserList);
        SelectedUser = dialogResult;
    }

    private async void EditUserCommandExecute()
    {
        var dialogResult =
            (User?)await CreateEditDialogProvider.ShowDialog(new EditUserDialogViewModel(ConfirmDialogProvider, SelectedUser!,
                CreateEditDialogProvider));

        if (dialogResult == null) return;

        _allUserList[_allUserList.FindIndex(p => p.Id == SelectedUser!.Id)] = dialogResult;
        DisplayedUserList = new ObservableCollection<User>(_allUserList);
        SelectedUser = dialogResult;
    }

    private async void DeleteUserCommandExecute()
    {
        if ((bool)await ConfirmDialogProvider.ShowDialog(new ConfirmDialogViewModel(ConfirmDialogProvider)))
        {
            await _userService.DeleteUser(SelectedUser!);
            GetData();
        }
    }

    private void SearchUsers()
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            _searchUserList = new List<User>(_allUserList
                .Where(a => a.Username.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)));

            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
        else
        {
            _searchUserList = null;
            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
    }

    private void SortingUsersByRole()
    {
        if (SelectedSortRole != null)
        {
            _sortUserList = new List<User>(_allUserList
                .Where(a => a.Role!.Name == SelectedSortRole.Name));

            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
        else
        {
            _sortUserList = null;
            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
    }

    private ObservableCollection<T> Merger<T>(IEnumerable<T> fullList, IEnumerable<IEnumerable<T>?> lists)
    {
        IEnumerable<T> resultList = lists.Where(list => list != null)
            .Aggregate(fullList, (current, list) => current.Intersect(list!));

        return new ObservableCollection<T>(resultList);
    }
}