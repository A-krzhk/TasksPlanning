
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskLibrary
{
    public class Task : INotifyPropertyChanged
    {
        private ObservableCollection<Task> Dep = new();
        private float _selfCost;
        private float _totalCost;
        private string _taskDependenciesString;

        public Task Clone()
        {
            return new Task(Id)
            {
                _selfCost = _selfCost,
                _totalCost = _totalCost,
                _taskDependenciesString = _taskDependenciesString,
                Dep = new ObservableCollection<Task>(Dep.ToArray()),
                Worker = Worker,
            };
        }
        
        public float SelfCost
        {
            get => _selfCost;
            private set
            {
                _selfCost = value;
                OnPropertyChanged();
            }
        }

        public float TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged();
            }
        }
        
        public string DependenciesString
        {
            get => _taskDependenciesString;
            set
            {
                _taskDependenciesString = Dep.Aggregate("", (current, dep) => current + $"   dep {dep.Id}|{dep.SelfCost}\n");
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Task> Depenedencies => Dep;

        public Employ Worker { get; private set; }

        public DateTime StartWork;
        public DateTime EndWork;
        
        public readonly uint Id;
        private static uint _nextUid;
        public static uint GetNextUid() => ++_nextUid;
        public string Name => Id.ToString();

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
        
        public Task(uint id, int cost = 0)
        {
            Id = id;
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

        public void AddDependency(Task task)
        {
            if (task == this)
                return;   
            Dep.Add(task);
            DependenciesString = DependenciesString;
        }
        
        public void RemoveDependency(Task task)
        {
            Dep.Remove(task);
            DependenciesString = DependenciesString;
        }
        
        public void SetWorker(Employ employ)
        {
            Worker = employ;
        }

        public void Print()
        {
            Console.WriteLine(ReturnPrint());
        }

        public string ReturnPrint()
        {
            var res = $"{ToString()}\n";
            foreach (var dep in Dep)
            {
                res += $"   dep {dep.Id}|{dep.SelfCost}\n";
            }

            return res;
        }

        public override string ToString()
        {
            return $"Task №{Id}, Self {SelfCost} Total {TotalCost}";
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


