using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskLibrary;
using TasksPlanning.ViewModels.Base;
using Task = TaskLibrary.Task;

namespace TasksPlanning.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public const string NewTaskPlaceHolder = "Введите стоимость новой таски (int)";
        private string _title= "Планировщик задач";
        private string _newTaskCost = NewTaskPlaceHolder;
        private Task _selectedTask;
        private Task _selectedOneClickTask;
        public TaskTracker TaskTracker { get; set; }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                Set(ref _selectedTask, value);
                OnPropertyChanged(nameof(Dependecies));
            }
        }
        
        public Task SelectedOneClickTask
        {
            get => _selectedOneClickTask;
            set => Set(ref _selectedOneClickTask, value);
        }

        public string NewTaskCost
        {
            get => _newTaskCost;
            set => Set(ref _newTaskCost, value);
        }
        
        private ICommand _taskDoubleClickCommand;
        public ICommand TaskDoubleClickCommand => _taskDoubleClickCommand ??= new RelayCommand<Task>(OnTaskDoubleClick);

        private ICommand _clickCommand;
        public ICommand Click => _clickCommand ??= new RelayCommand<Task>(OnTaskDoubleClick);
        private void OnTaskDoubleClick(Task task)
        {
            SelectedTask = task;
        }
        
        public MainWindowViewModel()
        {
            TaskTracker = new TaskTracker();
        }

        public ObservableCollection<Task>? Dependecies => _selectedTask?.Depenedencies;

        private RelayCommand addDependencyCommand;
        public RelayCommand AddDependencyCommand
        {
            get
            {
                return addDependencyCommand ??= new RelayCommand(obj =>
                {
                    if (obj is Task task && SelectedOneClickTask is not null && !task.Depenedencies.Contains(SelectedOneClickTask))
                    {
                        task.AddDependency(SelectedOneClickTask);
                    }
                    OnPropertyChanged(nameof(Dependecies));
                });
            }
        }
    }
}
