using System.Collections.ObjectModel;
using ITProcesses.Command;
using ITProcesses.JsonSaveInfo;

namespace ITProcesses.ViewModels;

public class MainViewModel : BaseViewModel
{
    private MainWindowViewModel _currentMainViewModel;
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
    public MainWindowViewModel CurrentMainViewModel
    {
        get => _currentMainViewModel;
        set
        {
            _currentMainViewModel = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Node> _nodes;
    public CommandHandler LogOutCommand => new(LogOutAsync);

    public ObservableCollection<Node> Nodes
    {
        get => _nodes;
        set
        {
            _nodes = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        Nodes = new ObservableCollection<Node>
        {
            new Node
            {
                Name ="Европа",
                Nodes = new ObservableCollection<Node>
                {
                    new Node {Name="Германия" },
                    new Node {Name="Франция" },
                    new Node
                    {
                        Name ="Великобритания",
                        Nodes = new ObservableCollection<Node>
                        {
                            new Node {Name="Англия" },
                            new Node {Name="Шотландия" },
                            new Node {Name="Уэльс" },
                            new Node {Name="Сев. Ирландия" },
                        }
                    }
                }
            },
            new Node
            {
                Name ="Азия",
                Nodes = new ObservableCollection<Node>
                {
                    new Node {Name="Китай" },
                    new Node {Name="Япония" },
                    new Node { Name ="Индия" }
                }
            },
            new Node { Name="Африка" },
            new Node { Name="Америка" },
            new Node { Name="Австралия" }
        };
    }

    public MainViewModel(MainWindowViewModel currentMainViewModel)
    {
        CurrentMainViewModel = currentMainViewModel;
    }

    private async void LogOutAsync()
    {
        try
        {
            CurrentMainViewModel.ChangeView(new LoginViewModel(CurrentMainViewModel));
            SaveInfo.CreateAppSettingsDefault();
        }
        catch
        {
            
        }
    }
}