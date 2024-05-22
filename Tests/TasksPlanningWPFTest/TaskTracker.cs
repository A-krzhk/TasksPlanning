using System.Threading.Tasks;

namespace TasksPlanningWPFTest
{
    public class TaskTracker
    {
        public readonly List<Task> AllTasks = new List<Task>();
        public List<Employ> AllEmploy = new List<Employ>();
        public IOrderedEnumerable<Task> OrderTasks;

        public void AddTask(Task task)
        {
            AllTasks.Add(task);
        }

        public Employ GetLowestEmploy()
        {
            var ordered = AllEmploy.OrderBy(employ => employ.CurrentWorkTime);
            return ordered.First();
        }

        public void RecalcTotalCost()
        {
            var zeroDep = AllTasks.Where(task => task.Dep.Count == 0);
            foreach (var task in zeroDep)
            {
                if (!AllTasks.Any(x => x.Dep.Contains(task)))
                {
                    task.TotalCost = task.SelfCost;
                    continue;
                }

                task.TotalCost = GetTotalCostInDependecies(task);
            }

            OrderTasks = AllTasks.OrderByDescending(task => task.TotalCost);
        }

        public Task GetHighestTask()
        {
            foreach (var task in OrderTasks)
            {
                if (task.Dep.Count != 0)
                {
                    continue;
                }
                AllTasks.Remove(task);
                return task;
            }
            return null!;
        }

        private float GetTotalCostInDependecies(Task task)
        {
            var total = 0f;
            foreach (var item in AllTasks.Where(x => x.Dep.Contains(task)))
            {
                total += GetTotalCostInDependecies(item);
            }

            task.TotalCost = total + task.SelfCost;
            return task.TotalCost;
        }

        public void RandomizeDep()
        {
            AllTasks[0].Dep.Add(AllTasks[1]);
            AllTasks[0].Dep.Add(AllTasks[2]);
            AllTasks[2].Dep.Add(AllTasks[4]);        
        }

        public void Print()
        {
            foreach (var task in AllTasks)
            {
                task.Print();
            }
        }

        public string PrintFirst()
        {
            foreach (var task in AllTasks)
            {
                return task.ReturnPrint();
            }
            return "gecnj";
        }
    }
}


