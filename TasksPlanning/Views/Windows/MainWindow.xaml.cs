using System.Collections.Specialized;
using System.Windows;
using TaskLibrary;
using Task = TaskLibrary.Task;

namespace TasksPlanning.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeWindow? _openedTreeWindow;

        public MainWindow()
        {
            InitializeComponent();
            WindowViewModel.TaskTracker.AddTask(new Task(11));
            WindowViewModel.TaskTracker.AddTask(new Task(51));
            WindowViewModel.TaskTracker.AddTask(new Task(1));
            WindowViewModel.TaskTracker.AddTask(new Task(15));
            WindowViewModel.TaskTracker.AddTask(new Task(11));
            WindowViewModel.TaskTracker.AllEmploy.Add(new Employ());
            WindowViewModel.TaskTracker.AllEmploy.Add(new Employ());
       
            WindowViewModel.TaskTracker.RandomizeDep();
            WindowViewModel.TaskTracker.AllTasks.CollectionChanged += OnPropertyChanged;
        }

        
        private void OnPropertyChanged(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _openedTreeWindow?.RefreshUi(WindowViewModel);
        }

        private void ButtonBase_OnClickAddTask(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(WindowViewModel.NewTaskCost, out var res))
            {
                var task = new Task(res);
                WindowViewModel.TaskTracker.AddTask(task);
                Label1.Content = $"Таска {task.Id} добавлена";
            }
            else
            {
                Label1.Content = "Введите целое число";
            }
        }

        private void PlainTasks(object sender, RoutedEventArgs e)
        {
            WindowViewModel.TaskTracker.PlanTasks();
            var window = new ScheduleWindow(WindowViewModel.TaskTracker.AllEmploy.Select(x => x.GetFullString()), WindowViewModel.TaskTracker.AllEmploy.SelectMany(employ => employ.WorkedTasks))
            {
                Owner = this
            };
            window.Show();
        }

        private void ShowTree(object sender, RoutedEventArgs e)
        {
            if (_openedTreeWindow == null)
            {
                _openedTreeWindow = new TreeWindow
                {
                    Owner = this
                };
                _openedTreeWindow.Closed += (o, args) => _openedTreeWindow = null; 
            }
           
            _openedTreeWindow.RefreshUi(WindowViewModel);
            _openedTreeWindow.Show();
        }

        private void AddWorker(object sender, RoutedEventArgs e)
        {
            WindowViewModel.TaskTracker.AllEmploy.Add(new Employ());
        }
    }
}