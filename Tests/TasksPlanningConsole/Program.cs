using TaskLibrary;
using Task = TaskLibrary.Task;

namespace TasksPlanningConsole
{
    internal class Program
    {
        private static TaskTracker _taskTracker;

        static void Main(string[] args)
        {
            _taskTracker = new TaskTracker();
            _taskTracker.AddTask(new Task(11));
            _taskTracker.AddTask(new Task(51));
            _taskTracker.AddTask(new Task(1));
            _taskTracker.AddTask(new Task(15));
            _taskTracker.AddTask(new Task(11));
            _taskTracker.AllEmploy.Add(new Employ());
            _taskTracker.AllEmploy.Add(new Employ());
            
            _taskTracker.RandomizeDep();
            _taskTracker.PlanTasks();
            _taskTracker.PrintWorkers();
        }
    }
}
