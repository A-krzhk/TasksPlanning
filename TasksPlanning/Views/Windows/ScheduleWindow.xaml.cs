using System.Collections.ObjectModel;
using System.Windows;
using TaskLibrary;
using Task = TaskLibrary.Task;

namespace TasksPlanning.Views.Windows;

public partial class ScheduleWindow : Window
{
    public ObservableCollection<Employ> TaskTrackerAllEmploy;

    public ScheduleWindow(ObservableCollection<Employ> taskTrackerAllEmploy, IEnumerable<Task> taskTrackerTaskToDistribution)
    {
        TaskTrackerAllEmploy = taskTrackerAllEmploy;
        InitializeComponent();
        EmployView.ItemsSource = taskTrackerAllEmploy;
        TasksView.ItemsSource = taskTrackerTaskToDistribution;
    }
}