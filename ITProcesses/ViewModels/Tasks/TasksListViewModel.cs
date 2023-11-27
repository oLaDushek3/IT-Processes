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

namespace ITProcesses.ViewModels;

public class TasksListViewModel : BaseViewModel
{
    #region Fields

    private readonly ITaskService _taskService = new TaskService();
    private readonly MainViewModel _currentMainViewModel;

    private ObservableCollection<Tasks> _tasksList;
    private Tasks _selectedTask;
    private string? _searchBox = string.Empty;
    private List<Tasks> _allTasks;
    private DateTime? _selectedDate = DateTime.Now;
    private List<Tasks> tasksEnumerable = new List<Tasks>();
    private List<Tasks> tasksFromDatePickerList = new List<Tasks>();

    #endregion


    #region Properties

    public ObservableCollection<Tasks> TasksList
    {
        get => _tasksList;
        set
        {
            _tasksList = value;
            OnPropertyChanged();
        }
    }

    public string SearchBox
    {
        get => _searchBox;
        set
        {
            _searchBox = value;
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

    #endregion

    //Constructor
    public TasksListViewModel(MainViewModel currentMainViewModel)
    {
        // _taskService = new TaskService();
        _currentMainViewModel = currentMainViewModel;
        _tasksList = new ObservableCollectionListSource<Tasks>();

        GetData();
    }

    public CommandHandler OpenTaskCommand => new(obg => OpenTask(obg as Tasks));

    public CommandHandler ClearDatePickerCommand => new(ClearDatePicker);

    // public CommandHandler ClearDatePickerCommand => new(ClearDatePicker);

    //Methods
    private async void GetData()
    {
        _allTasks = await _taskService.GetTasksByProject(_currentMainViewModel.CurrentProject.Id);
        TasksList = new ObservableCollection<Tasks>(_allTasks);
    }

    private void OpenTask(Tasks selectedTask)
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(selectedTask.Id, _currentMainViewModel));
    }

    private void SearchInfoFromSearchBox()
    {
        if (!string.IsNullOrEmpty(_searchBox))
        {
            tasksEnumerable = new List<Tasks>(_allTasks
                .Where(a => a.Name.ToLower().Contains(_searchBox.ToLower())));
            TasksList = null;

            TasksList = Merger(new List<List<Tasks>> { tasksEnumerable, tasksFromDatePickerList });
        }
        else
        {
            GetData();
        }
    }

    private void SearchDateFromDatePicker()
    {
        if (_selectedDate != null)
        {
            tasksFromDatePickerList =
                new List<Tasks>(_allTasks
                    .Where(a => a.DateCreateTimestamp.Date == _selectedDate));
            TasksList = null;
            
            TasksList = Merger(new List<List<Tasks>> { tasksFromDatePickerList, tasksEnumerable });
        }
        else
        {
            GetData();
        }
    }

    private ObservableCollection<T> Merger<T>(List<List<T>> lists)
    {
        IEnumerable<T> resultList = lists.First();

        foreach (List<T> list in lists)
        {
            resultList.Intersect(list);
        }

        return new ObservableCollection<T>(resultList);
    }

    private void ClearDatePicker(object o)
    {
        _selectedDate = null;
        tasksFromDatePickerList = null;
        GetData();
    }
}