using System.Collections.Generic;
namespace TaskLibrary
{
    public class Employ
    {
        private readonly List<Task> _workedTasks = new List<Task>();
        public float CurrentWorkTime => _workedTasks.Sum(task => task.SelfCost);
        public string Name { get; set; } = (new Random().Next()).ToString("D")[..3];
        public float EndLastTask = 0;
        public IReadOnlyList<Task> WorkedTasks => _workedTasks;
        public void Clear()
        {
            EndLastTask = 0;
            _workedTasks.Clear();
        }
        
        public void AddWorkTask(Task task)
        {
            var start = EndLastTask > task.StartWork ? EndLastTask : task.StartWork;
            task.StartWork = start;
            task.EndWork = start + task.SelfCost;
            EndLastTask = task.EndWork;
            
            _workedTasks.Add(task);
        }
        
        public void Print()
        {
            Console.WriteLine(GetFullString());
        }

        public string GetFullString()
        {
            var res = $"Worked AllTime: {EndLastTask}";
            foreach (var task in _workedTasks)
            {
                res += $"\n {task.Id} ({task.StartWork} | {task.EndWork})";
            }

            return res;
        }
    
        public override string ToString()
        {
            return $"Worker ID: {Name}";
        }
    }
}


