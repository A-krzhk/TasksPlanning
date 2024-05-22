
namespace TasksPlanningWPFTest
{
    public class Task
    {
        public List<Task> Dep = new List<Task>();
        public float SelfCost { get; private set; }
        public float TotalCost { get; set; }
        public DateTime StartWork;
        public DateTime EndWork;
        
        
        public Employ Worker;
        public uint Id;
        private static uint _nextUid;
        public static uint GetNextUid() => ++_nextUid;

        public Task(int cost = 0)
        {
            Id = GetNextUid();
            if (cost == 0)
            {
                var rand = new Random();
                SelfCost = rand.Next(1, 10);
            }
            else
            {
                SelfCost = cost;
            }
            
        }

        public void Print()
        {
            var res = $"Task №{Id}, Self {SelfCost} Total {TotalCost} \n";
            foreach (var dep in Dep)
            {
                res += $"   dep {dep.Id}|{dep.SelfCost}\n";
            }

            Console.WriteLine(res);
        }

        public string  ReturnPrint()
        {
            var res = $"Task №{Id}, Self {SelfCost} Total {TotalCost} \n";
            foreach (var dep in Dep)
            {
                res += $"   dep {dep.Id}|{dep.SelfCost}\n";
            }

            return res;
        }

        public static void SortDemucron(TaskTracker taskTracker)
        {
            var first = true;
            while (taskTracker.AllTasks.Count != 0)
            {
                taskTracker.RecalcTotalCost();
                if (first)
                {
                    taskTracker.Print();
                    first = false;
                }
                var importantTask = taskTracker.GetHighestTask();
                
                var employ = taskTracker.GetLowestEmploy();
                employ.AddWorkTask(importantTask);
                importantTask.Worker = employ;

                foreach (var task2 in taskTracker.AllTasks.Where(task1 => task1.Dep.Contains(importantTask)))
                {
                    task2.StartWork = importantTask.EndWork;
                    task2.Dep.Remove(importantTask);
                }
            }
        }

    }
}


