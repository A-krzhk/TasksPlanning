
namespace TaskLibrary
{
    public class Employ
    {
        private readonly List<Task> _workedTasks = new List<Task>();
        public float CurrentWorkTime => _workedTasks.Sum(task => task.SelfCost);
        public readonly string Name = (new Random().Next()).ToString("D")[..3];
        public DateTime EndLastTask = DateTime.Parse("00:00");

        public IReadOnlyList<Task> WorkedTasks => _workedTasks;

        public void Clear()
        {
            EndLastTask = DateTime.Parse("00:00");
            _workedTasks.Clear();
        }
        
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
            Console.WriteLine(GetFullString());
        }

        public string GetFullString()
        {
            var res = $"Worked AllTime: {EndLastTask.TimeOfDay.TotalHours}";
            foreach (var task in _workedTasks)
            {
                res += $"\n {task.Id} ({task.StartWork.TimeOfDay:hh} | {task.EndWork.TimeOfDay:hh})";
            }

            return res;
        }
    
        public override string ToString()
        {
            return $"Worker: {Name}";
        }
    }
}


