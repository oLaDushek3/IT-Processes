using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ITProcesses.Command;
using ITProcesses.Dialog;
using ITProcesses.Models;
using ITProcesses.Services;
using Type = ITProcesses.Models.Type;

namespace ITProcesses.ViewModels;

public class SelectionBeforeTaskDialogViewModel : BaseViewModel
{
    #region Field

    private static readonly ItprocessesContext Context = new();

    private readonly ITaskService _taskService = new TaskService(Context);
    private readonly ITaskService _userService = new TaskService(Context);
    private readonly DialogProvider _currentDialogProvider;

    private List<Type> _roleList;
    private List<Tasks> _allTaskList;
    private ObservableCollection<Tasks> _displayedTaskList;

    //Search and sort
    private string _searchString;
    private List<Tasks>? _searchTaskList;

    private Type? _selectedSortType;
    private List<Tasks>? _sortTaskList;

    #endregion

    #region Properties

    public List<Type> TypeList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Tasks> DisplayedTaskList
    {
        get => _displayedTaskList;
        set
        {
            _displayedTaskList = value;
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
                _searchTaskList = null;
            TaskSearch();
        }
    }

    public Type? SelectedSortType
    {
        get => _selectedSortType;
        set
        {
            _selectedSortType = value;
            OnPropertyChanged();
            TaskSort();
        }
    }

    #endregion

    //Commands
    public CommandHandler AcceptSelectCommand =>
        new(selectedTasks => _currentDialogProvider.CloseDialog(selectedTasks as Tasks),
            selectedTasks => selectedTasks != null);

    public CommandHandler CancelCommand => new(_ => _currentDialogProvider.CloseDialog(null));

    public CommandHandler ClearSort => new(_ =>
    {
        SelectedSortType = null;
        _sortTaskList = null;
    });

    //Constructor
    public SelectionBeforeTaskDialogViewModel(DialogProvider currentDialogProvider)
    {
        _currentDialogProvider = currentDialogProvider;
        GetData();
    }

    //Methods
    private async void GetData()
    {
        _allTaskList = await _taskService.GetAllTask();
        DisplayedTaskList = new ObservableCollection<Tasks>(_allTaskList);

        TypeList = await _userService.GetAllTypes();
    }

    private void TaskSearch()
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            _searchTaskList = new List<Tasks>(_allTaskList
                .Where(a => a.Name.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)));

            DisplayedTaskList = Merger(_allTaskList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
        else
        {
            DisplayedTaskList = new ObservableCollection<Tasks>(_allTaskList);
        }
    }

    private void TaskSort()
    {
        if (SelectedSortType != null)
        {
            _sortTaskList = new List<Tasks>(_allTaskList
                .Where(a => a.Type!.Name == SelectedSortType.Name));

            DisplayedTaskList = Merger(_allTaskList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
        else
        {
            DisplayedTaskList = new ObservableCollection<Tasks>(_allTaskList);
        }
    }

    private ObservableCollection<T> Merger<T>(IEnumerable<T> fullList, IEnumerable<IEnumerable<T>?> lists)
    {
        IEnumerable<T> resultList = lists.Where(list => list != null)
            .Aggregate(fullList, (current, list) => current.Intersect(list!));

        return new ObservableCollection<T>(resultList);
    }
}