﻿using Microsoft.Msagl.DebugHelpers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TaskLibrary;
using TasksPlanning.ViewModels.Base;
using Task = TaskLibrary.Task;


namespace TasksPlanning.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public const string NewTaskPlaceHolder = "Enter the duration of the task";
        private string _newTaskCost = NewTaskPlaceHolder;
        
        private Task? _selectedTask;
        private Task? _selectedOneClickTask;
        private Employ? _selectedEmploy;
        private TaskTracker _endAllTasks;
        public TaskTracker TaskTracker { get; set; }

        public TaskTracker? EndAllTasks
        {
            get => _endAllTasks;
            set => Set(ref _endAllTasks, value);
        }

        public Task? SelectedTask
        {
            get => _selectedTask;
            set
            {
                Set(ref _selectedTask, value);
                OnPropertyChanged(nameof(Dependecies));
            }
        }
        
        public Employ? SelectedEmploy
        {
            get => _selectedEmploy;
            set
            {
                Set(ref _selectedEmploy, value);
                OnPropertyChanged(nameof(TaskTracker.AllEmploy));
            }
        }
        
        public Task? SelectedOneClickTask
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

        private RelayCommand? _addDependencyCommand;
        public RelayCommand AddDependencyCommand
        {
            get
            {
                return _addDependencyCommand ??= new RelayCommand(obj =>
                {
                    if (obj is Task task && 
                        SelectedOneClickTask is not null)

                    {
                        var contains = task.Depenedencies.Contains(SelectedOneClickTask) ||
                            SelectedOneClickTask.Depenedencies.Contains(task) || 
                            CheckCircual(SelectedOneClickTask, task) || CheckCircual(task, SelectedOneClickTask);
                        if (contains)
                        {
                            MessageBox.Show("The dependency is not added because you are trying to make a cyclic dependency, or this dependence already transmitted by transitivity");
                            return;
                        }

                        task.AddDependency(SelectedOneClickTask);
                    }

                    OnPropertyChanged(nameof(Dependecies));
                });
            }
        }

        private bool CheckCircual(Task selfTask, Task targetTask)
        {
            
            foreach (var item in selfTask.Depenedencies)
            {
                if (selfTask.Depenedencies.Contains(targetTask) || CheckCircual(item, targetTask) || item == targetTask)
                {
                    return true;
                }
            }
            return false;
        }


        private RelayCommand? _removeWorkerBinding;
        public RelayCommand RemoveWorkerBinding  {
            get
            {
                return _removeWorkerBinding ??= new RelayCommand(obj =>
                {
                    if (SelectedEmploy == null)
                        return;
                    
                    TaskTracker.AllEmploy.Remove(SelectedEmploy);
                    SelectedEmploy = null;
                    OnPropertyChanged(nameof(SelectedEmploy));
                });
            }
        }
    }
}
