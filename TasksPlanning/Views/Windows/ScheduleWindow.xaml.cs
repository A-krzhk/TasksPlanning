using System.Windows;
using Task = TaskLibrary.Task;

namespace TasksPlanning.Views.Windows;

public partial class ScheduleWindow : Window
{

    public ScheduleWindow(IEnumerable<string> taskTrackerAllEmploy, IEnumerable<Task> taskTrackerTaskToDistribution)
    {
        InitializeComponent();
        EmployView.ItemsSource = taskTrackerAllEmploy;
        TasksView.ItemsSource = taskTrackerTaskToDistribution;
    }
}