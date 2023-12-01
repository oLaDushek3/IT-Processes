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

    private readonly ItprocessesContext _context = new();
    private readonly ITaskService _taskService;

    private ObservableCollection<Tasks> _displayedDisplayedTaskList;
    private List<Tasks> _allTasksList;
    private Tasks _selectedTask;

    //Search and sort
    private string _searchString;
    private List<Tasks>? _searchTaskList;

    private DateTime? _selectedDate;
    private List<Tasks>? _sortByDateTaskList;

    private List<Type> _typeList;
    private Type? _selectedType;
    private List<Tasks>? _sortByTypeTaskList;

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
            SearchTasks();
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged();
            SortingTasksByDate();
        }
    }

    public List<Type> TypeList
    {
        get => _typeList;
        set
        {
            _typeList = value;
            OnPropertyChanged();
        }
    }

    public Type? SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged();
            SortingTasksByType();
        }
    }

    #endregion

    //Commands
    public CommandHandler OpenTaskCommand => new(obg => OpenTask(obg as Tasks));

    public CommandHandler ClearDateSort => new(_ => SelectedDate = null);

    public CommandHandler ClearTypeSort => new(_ => SelectedType = null);

    public CommandHandler CreateTaskCommand => new(_ => CreateTaskCommandExecute());

    public CommandHandler DeleteTaskCommand =>
        new(selectedTask => DeleteTaskCommandExecute(selectedTask as Tasks), selectedTask => selectedTask != null);

    //Constructor
    public TasksListViewModel(MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService(_context);
        _currentMainViewModel = currentMainViewModel;
        _displayedDisplayedTaskList = new ObservableCollectionListSource<Tasks>();

        GetData();
    }

    //Methods
    private async void GetData()
    {
        _allTasksList = await _taskService.GetTasksByProject(_currentMainViewModel.CurrentProject.Id);
        DisplayedTaskList = new ObservableCollection<Tasks>(_allTasksList);
        TypeList = await _taskService.GetAllTypes();
    }

    private async void CreateTaskCommandExecute()
    {
        var dialogResult =
            (Tasks?)await _currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
                new CreateTaskDialogViewModel(_currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider,
                    _currentMainViewModel));

        if (dialogResult == null) return;

        _allTasksList.Add(dialogResult);
        DisplayedTaskList = new ObservableCollection<Tasks>(_allTasksList);
    }

    private async void DeleteTaskCommandExecute(Tasks selectedTask)
    {
        if ((bool)await _currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider.ShowDialog(
                new ConfirmDialogViewModel(_currentMainViewModel.CurrentMainWindowViewModel.MainDialogProvider)))
        {
            await _taskService.DeleteTask(await _taskService.GetTaskById(selectedTask.Id));
            GetData();
        }
    }

    private void OpenTask(Tasks selectedTask)
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(selectedTask.Id, _currentMainViewModel));
    }

    private void SearchTasks()
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            _searchTaskList = new List<Tasks>(_allTasksList
                .Where(a => a.Name.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)));

            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
        else
        {
            _searchTaskList = null;
            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
    }

    private void SortingTasksByDate()
    {
        if (SelectedDate != null)
        {
            _sortByDateTaskList = new List<Tasks>(_allTasksList
                .Where(a => a.DateCreateTimestamp.Date == _selectedDate));

            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
        else
        {
            _sortByDateTaskList = null;
            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
    }

    private void SortingTasksByType()
    {
        if (SelectedType != null)
        {
            _sortByTypeTaskList = new List<Tasks>(_allTasksList
                .Where(a => a.Type!.Name == SelectedType.Name));

            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
        else
        {
            _sortByTypeTaskList = null;
            DisplayedTaskList = Merger(_allTasksList,
                new List<List<Tasks>> { _searchTaskList, _sortByDateTaskList, _sortByTypeTaskList });
        }
    }

    private ObservableCollection<T> Merger<T>(IEnumerable<T> fullList, IEnumerable<IEnumerable<T>?> lists)
    {
        IEnumerable<T> resultList = lists.Where(list => list != null)
            .Aggregate(fullList, (current, list) => current.Intersect(list!));

        return new ObservableCollection<T>(resultList);
    }
}