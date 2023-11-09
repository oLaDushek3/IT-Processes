using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ITProcesses.Command;
using ITProcesses.Models;
using ITProcesses.Services;
using ITProcesses.Views;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITProcesses.ViewModels;

public class TasksListViewModel : BaseViewModel
{
    #region Fields

    private ITaskService _taskService;
    private MainViewModel _currentMainViewModel;

    private ObservableCollection<Tasks> _tasksList;
    private Tasks _selectedTask;
    private string _searchBox = string.Empty;

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
        }
    }

    #endregion

    //Constructor
    public TasksListViewModel(MainViewModel currentMainViewModel)
    {
        _taskService = new TaskService();
        _currentMainViewModel = currentMainViewModel;
        _tasksList = new ObservableCollectionListSource<Tasks>();

        GetData();
    }

    public CommandHandler<Tasks> OpenTaskCommand => new(OpenTask);

    //Methods
    private async void GetData()
    {
        TasksList = new ObservableCollection<Tasks>(await _taskService.GetAllTask());
    }

    private void OpenTask(Tasks selectedTask)
    {
        _currentMainViewModel.ChangeView(new TaskViewModel(selectedTask.Id, _currentMainViewModel));
    }

    private async void SearchInfo()
    {
        if (_searchBox != string.Empty)
        {
            var allTaskList = await _taskService.GetAllTask();

            ObservableCollection<Tasks> tasksEnumerable = (ObservableCollection<Tasks>)allTaskList
                .Where(a => a.Name.Contains(_searchBox));
            TasksList = null;
            TasksList = tasksEnumerable;
        }
        else
        {
            GetData();
        }
    }
}