
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskLibrary
{
    public class TaskTracker : INotifyPropertyChanged
    {
        private ObservableCollection<Task> _allTasks = new ObservableCollection<Task>();
        private ObservableCollection<Employ> _allEmploy = new ObservableCollection<Employ>();
        private List<Task> _taskToDistribution = new List<Task>();

        public ObservableCollection<Task> AllTasks => _allTasks;

        public ObservableCollection<Employ> AllEmploy
        {
            get => _allEmploy;
            set => _allEmploy = value;
        }

        public List<Task> TaskToDistribution => _taskToDistribution;

        public void AddTask(Task task)
        {
            _allTasks.Add(task);
        }

        public Employ GetLowestEmploy()
        {
            var ordered = _allEmploy.OrderBy(employ => employ.CurrentWorkTime);
            return ordered.First();
        }

        public void RecalculateTotalCost()
        {
            var zeroDep = _taskToDistribution.Where(task => task.Depenedencies.Count == 0);
            foreach (var task in zeroDep)
            {
                if (!_taskToDistribution.Any(x => x.Depenedencies.Contains(task)))
                {
                    task.TotalCost = task.SelfCost;
                    continue;
                }

                task.TotalCost = GetTotalCostInDependecies(task);
            }
        }

        public Task GetHighestTask()
        {
            foreach (var task in _taskToDistribution.OrderByDescending(task => task.TotalCost))
            {
                if (task.Depenedencies.Count != 0)
                {
                    continue;
                }
                _taskToDistribution.Remove(task);
                return task;
            }
            return null!;
        }

        private float GetTotalCostInDependecies(Task task)
        {
            var total = 0f;
            foreach (var item in _taskToDistribution.Where(x => x.Depenedencies.Contains(task)))
            {
                total += GetTotalCostInDependecies(item);
            }

            task.TotalCost = total + task.SelfCost;
            return task.TotalCost;
        }

        public void RandomizeDep()
        {
            _allTasks[0].AddDependency(_allTasks[1]);
            _allTasks[0].AddDependency(_allTasks[2]);
            _allTasks[2].AddDependency(_allTasks[4]);        
        }

        public void Print()
        {
            foreach (var task in _allTasks)
            {
                task.Print();
            }
        }

        public string PrintFirst()
        {
            return _allTasks.First().ReturnPrint();
        }
        
        public void PlanTasks()
        {
            var first = true;
            _taskToDistribution.Clear();
            foreach (var task in _allTasks)
            {
                _taskToDistribution.Add(task);
            }
            
            while (_taskToDistribution.Count != 0)
            {
                RecalculateTotalCost();
                if (first)
                {
                    Print();
                    first = false;
                }
                var importantTask = GetHighestTask();
                if (importantTask is null)
                {
                    return;
                }
                var employ = GetLowestEmploy();
                employ.AddWorkTask(importantTask);
                importantTask.SetWorker(employ);

                foreach (var task2 in _taskToDistribution.Where(task1 => task1.Depenedencies.Contains(importantTask)))
                {
                    task2.StartWork = importantTask.EndWork;
                    task2.RemoveDependency(importantTask);
                }
            }
        }
        
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;
            
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}


