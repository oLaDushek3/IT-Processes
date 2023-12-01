using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using Type = ITProcesses.Models.Type;

namespace ITProcesses.ViewModels;

public class SelectionUserToTaskDialogViewModel : BaseViewModel
{
    #region Field

    private readonly ItprocessesContext _context = new();

    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly DialogProvider _currentDialogProvider;

    private List<Role> _roleList;
    private List<User> _allUserList;
    private ObservableCollection<User> _displayedUserList;

    //Search and sort
    private string _searchString;
    private List<User>? _searchUserList;

    private Role? _selectedSortRole;
    private List<User>? _sortUserList;

    #endregion

    #region Properties

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
    public CommandHandler AcceptSelectCommand =>
        new(selectedUsers => _currentDialogProvider.CloseDialog((selectedUsers as IList).Cast<User>().ToList()),
            selectedUsers => selectedUsers != null && (selectedUsers as IList).Count != 0);

    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler ClearSortByRoleCommand => new(_ => SelectedSortRole = null);

    //Constructor
    public SelectionUserToTaskDialogViewModel(DialogProvider currentDialogProvider, Guid taskId)
    {
        _taskService = new TaskService(_context);
        _userService = new UserService(_context);
        _currentDialogProvider = currentDialogProvider;
        GetData(taskId);
    }

    //Methods
    private async void GetData(Guid taskId)
    {
        _allUserList = await _taskService.GetUsersNotParticipatingInTask(taskId);
        DisplayedUserList = new ObservableCollection<User>(_allUserList);

        RoleList = await _userService.GetAllRoles();
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