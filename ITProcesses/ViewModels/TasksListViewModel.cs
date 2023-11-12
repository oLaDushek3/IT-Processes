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
    private string _searchBox = string.Empty;
    private List<Tasks> _allTasks;
    private DateTime _selectedDate = DateTime.Now;

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

    public DateTime SelectedDate
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

    public CommandHandler<Tasks> OpenTaskCommand => new(OpenTask);

    //Methods
    private async void GetData()
    {
        _allTasks = await _taskService.GetAllTask();
        TasksList = new ObservableCollection<Tasks>(_allTasks);
    }

    private void OpenTask(Tasks selectedTask)
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(selectedTask.Id, _currentMainViewModel));
    }

    private void SearchInfoFromSearchBox()
    {
        if (_searchBox != string.Empty)
        {
            ObservableCollection<Tasks> tasksEnumerable = new ObservableCollection<Tasks>(_allTasks
                .Where(a => a.Name.Contains(_searchBox)));
            TasksList = null;
            TasksList = tasksEnumerable;
        }
        else
        {
            GetData();
        }
    }

    private void SearchDateFromDatePicker()
    {
        if (_selectedDate != new DateTime())
        {
            ObservableCollection<Tasks> tasksFromDatePickerList =
                new ObservableCollection<Tasks>(_allTasks
                    .Where(a => a.DateCreateTimestamp.Date == _selectedDate));
            TasksList = null;
            TasksList = tasksFromDatePickerList;
        }
        else
        {
            GetData();
        }
    }

    private void Merger()
    {
    }
}