using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using ITProcesses.Command;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.Views;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Type = ITProcesses.Models.Type;

namespace ITProcesses.ViewModels;

public class TasksListViewModel : BaseViewModel
{
    #region Fields
    
    private readonly MainViewModel _currentMainViewModel;
    
    private readonly ItprocessesContext _context = new ();
    private readonly ITaskService _taskService;

    private ObservableCollection<Tasks> _displayedDisplayedTaskList;
    private List<Tasks> _allTasksList;
    private Tasks _selectedTask;
    
    //Search and sort
    private string _searchString;
    private List<Type> _roleList;
    private DateTime? _selectedDate = DateTime.Now;
    private List<Tasks> _searchTaskList;
    private List<Tasks> _sortTaskList;

    #endregion
    
    #region Properties

    public ObservableCollection<Tasks> DisplayedTaskList
    {
        get => _displayedDisplayedTaskList;
        set
        {
            _displayedDisplayedTaskList = value;
            OnPropertyChanged();
        }
    }

    public string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            OnPropertyChanged();
            SearchInfoFromSearchBox();
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged();
            SearchDateFromDatePicker();
        }
    }
    
    public List<Type> TypeList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Constructor
    public TasksListViewModel(MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService(_context);
        _currentMainViewModel = currentMainViewModel;
        _displayedDisplayedTaskList = new ObservableCollectionListSource<Tasks>();

        GetData();
    }

    public CommandHandler OpenTaskCommand => new(obg => OpenTask(obg as Tasks));
    
    public CommandHandler ClearDateSort => new(_ => SelectedDate = null);

    //Methods
    private async void GetData()
    {
        _allTasksList = await _taskService.GetTasksByProject(_currentMainViewModel.CurrentProject.Id);
        DisplayedTaskList = new ObservableCollection<Tasks>(_allTasksList);
    }

    private void OpenTask(Tasks selectedTask)
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(selectedTask.Id, _currentMainViewModel));
    }

    private void SearchInfoFromSearchBox()
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            _searchTaskList = new List<Tasks>(_allTasksList
                .Where(a => a.Name.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)));

            DisplayedTaskList = Merger(_allTasksList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
        else
        {
            _searchTaskList = null;
            DisplayedTaskList = Merger(_allTasksList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
    }

    private void SearchDateFromDatePicker()
    {
        if (SelectedDate != null)
        {
            _sortTaskList = new List<Tasks>(_allTasksList
                .Where(a => a.DateCreateTimestamp.Date == _selectedDate));

            DisplayedTaskList = Merger(_allTasksList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
        else
        {
            _sortTaskList = null;
            DisplayedTaskList = Merger(_allTasksList, new List<List<Tasks>> { _searchTaskList, _sortTaskList });
        }
    }
    
    private ObservableCollection<T> Merger<T>(IEnumerable<T> fullList, IEnumerable<IEnumerable<T>?> lists)
    {
        IEnumerable<T> resultList = lists.Where(list => list != null)
            .Aggregate(fullList, (current, list) => current.Intersect(list!));

        return new ObservableCollection<T>(resultList);
    }

    private void ClearDatePicker(object o)
    {
        SelectedDate = null;
        _sortTaskList = null;
    }
}