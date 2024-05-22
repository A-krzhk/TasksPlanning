using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksPlanning.ViewModels.Base;

namespace TasksPlanning.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        private string _title="Планировщик задач";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
    }
}
