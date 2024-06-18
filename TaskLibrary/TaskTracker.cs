
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

        private Dictionary<string, TempTask> _tempTasks = new Dictionary<string, TempTask>();
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
            var zeroDep = _tempTasks.Values.Where(task => task.Dependencies.Count == 0);
            foreach (var task in zeroDep)
            {
                TempTask tempTask;
                if (!_tempTasks.Values.Any(x => x.Dependencies.Contains(task.Uid)))
                {
                    tempTask = _tempTasks[task.Uid];
                    tempTask.TotalCost = task.SelfCost;
                    _tempTasks[task.Uid] = tempTask;
                    continue;
                }
                tempTask = _tempTasks[task.Uid];
                tempTask.TotalCost = GetTotalCostInDependecies(ref task);
                _tempTasks[task.Uid] = tempTask;
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
        
        public void PrintWorkers()
        {
            foreach (var task in AllEmploy)
            {
                task.Print();
            }
        }
        
        public void PlanTasks()
        {
            _taskToDistribution.Clear();
            _tempTasks.Clear();
            
            foreach (var task in _allTasks)
            {
                //_taskToDistribution.Add(task);
                _tempTasks.Add(task.Name, new TempTask(task));
            }
            
            while (_tempTasks.Count != 0)
            {
                RecalculateTotalCost();

                var importantTask = GetHighestTask();
                if (importantTask is null)
                    break;
                
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

        public struct TempTask
        {
            public string Uid;
            public float SelfCost;
            public float TotalCost;
            public List<string> Dependencies;

            public TempTask(Task task)
            {
                TotalCost = task.TotalCost;
                SelfCost = task.SelfCost;
                Uid = task.Name;
                Dependencies = new List<string>();
                foreach (var depenedency in task.Depenedencies)
                {
                    Dependencies.Add(depenedency.Name);
                }
            }
        }
    }
}


