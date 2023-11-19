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

namespace ITProcesses.ViewModels;

public class UserSelectionDialogViewModel : BaseViewModel
{
    #region Field

    private readonly DialogProvider _currentDialogProvider;
    private readonly IUserService _userService = new UserService();

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
            UserSearch();
        }
    }

    public Role? SelectedSortRole
    {
        get => _selectedSortRole;
        set
        {
            _selectedSortRole = value;
            OnPropertyChanged();
            UserSort();
        }
    }

    #endregion

    //Commands
    public CommandHandler AcceptSelectCommand =>
        new(selectedUsers => _currentDialogProvider.CloseDialog((selectedUsers as IList).Cast<User>().ToList()), 
            selectedUsers => selectedUsers != null && (selectedUsers as IList).Count != 0);

    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler ClearSort => new(_ =>
    {
        SelectedSortRole = null;
        _sortUserList = null;
    });

    //Constructor
    public UserSelectionDialogViewModel(DialogProvider currentDialogProvider)
    {
        _currentDialogProvider = currentDialogProvider;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        _allUserList = await _userService.GetAllUsers();
        DisplayedUserList = new ObservableCollection<User>(_allUserList);
        RoleList = await _userService.GetAllRoles();
    }

    private void UserSearch()
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            _searchUserList = new List<User>(_allUserList
                .Where(a => a.Username.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)));

            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
        else
        {
            DisplayedUserList = new ObservableCollection<User>(_allUserList);
        }
    }

    private void UserSort()
    {
        if (SelectedSortRole != null)
        {
            _sortUserList = new List<User>(_allUserList
                .Where(a => a.Role!.Name == SelectedSortRole.Name));

            DisplayedUserList = Merger(_allUserList, new List<List<User>> { _searchUserList, _sortUserList });
        }
        else
        {
            DisplayedUserList = new ObservableCollection<User>(_allUserList);
        }
    }

    private ObservableCollection<T> Merger<T>(IEnumerable<T> fullList, IEnumerable<IEnumerable<T>?> lists)
    {
        IEnumerable<T> resultList = lists.Where(list => list != null)
            .Aggregate(fullList, (current, list) => current.Intersect(list!));

        return new ObservableCollection<T>(resultList);
    }
}