using Microsoft.Msagl.Drawing;
using System.Windows;
using TasksPlanning.ViewModels;


namespace TasksPlanning.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TreeWindow.xaml
    /// </summary>
    public partial class TreeWindow : Window
    {
        public TreeWindow()
        {
            InitializeComponent();
        }

        public void RefreshUi(MainWindowViewModel viewModel)
        {
            Graph graph = new();
            foreach(var task in viewModel.TaskTracker.AllTasks)
            {
                graph.AddNode(task.Name);
                foreach (var dep in task.Depenedencies)
                {
                    graph.AddEdge(dep.Name, task.Name);
                }
            }
            GraphLayouts.Graph = graph;
            GraphLayouts.UpdateLayout();
        }
    }
}
