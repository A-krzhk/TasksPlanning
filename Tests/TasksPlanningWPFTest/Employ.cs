using System.Globalization;

namespace TasksPlanningWPFTest
{
    public class Employ
    {
        private readonly List<Task> _workedTasks = new List<Task>();
        public float CurrentWorkTime => _workedTasks.Sum(task => task.SelfCost);
        public readonly string Name = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        public DateTime EndLastTask = System.DateTime.Parse("00:00");
        
        public void AddWorkTask(Task task)
        {
            var start = EndLastTask.TimeOfDay > task.StartWork.TimeOfDay ? EndLastTask : task.StartWork;
            task.StartWork = start;
            task.EndWork = start + TimeSpan.FromHours(task.SelfCost);
            EndLastTask = task.EndWork;
            
            _workedTasks.Add(task);
        }
        
        public void Print()
        {
            var res = $"Worked AllTime: {EndLastTask.TimeOfDay.TotalHours}";
            foreach (var task in _workedTasks)
            {
                res += $"\n {task.Id} ({task.StartWork.TimeOfDay:hh} | {task.EndWork.TimeOfDay:hh})";
            }

            Console.WriteLine(res);
        }
    }
}


