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
        }

        private void ShowTree(object sender, RoutedEventArgs e)
        {
            TreeWindow treeWindow = new TreeWindow();
            foreach(var task in WindowViewModel.TaskTracker.AllTasks)
            {
                treeWindow.graph.AddNode(task.Name);
                foreach (var dep in task.Depenedencies)
                {
                    treeWindow.graph.AddEdge(dep.Name, task.Name);
                }
            }
            treeWindow.Show();
        }

        
    }
}