using System.Windows;

namespace TasksPlanningWPFTest
{
    public partial class MainWindow : Window
    {
        private readonly TaskTracker _taskTracker = new();
    
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputTextBox.Text, out var res))
            {
                var task = new Task(res);
                _taskTracker.AddTask(task);
                ListView.Items.Add(task);
                Label1.Content = $"Таска {task.Id} добавлена";
            }
            else
            {
                Label1.Content = "Введите целое число";
            }
             
            //ListView.DataContext = _taskTracker.AllTasks.Select(t => t.TaskName).ToArray();
        }

        private void ListView_OnSelected(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem is not Task task)
            {
                TaskId.Content = "Not selected";
                TaskDependecies.Items.Clear();
                AddDepBtn.IsEnabled = false;
                return;
            }
            AddDepBtn.IsEnabled = true;
            TaskId.Content = task.Id;
            TaskDependecies.Items.Clear();
            foreach (var t in task.Dep)
            {
                TaskDependecies.Items.Add(t);
            }
        }

        private void AddDepBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem is not Task task)
            {
                return;
            }

            if (int.TryParse(AddDepIdTxtBox.Text, out var res))
            {
                var taskToAdd = _taskTracker.AllTasks.FirstOrDefault(x => x.Id == res);
                if (taskToAdd != null) 
                    task.Dep.Add(taskToAdd);
                
                TaskDependecies.Items.Clear();
                foreach (var t in task.Dep)
                {
                    TaskDependecies.Items.Add(t);
                }
            }
        }
    }
}