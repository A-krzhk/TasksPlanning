using System.Collections.ObjectModel;
using System.Windows;
using Employ = TaskLibrary.Employ;
using Task = TaskLibrary.Task;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Diagnostics;

namespace TasksPlanning.Views.Windows;
public partial class ScheduleWindow : System.Windows.Window
{
    private readonly IReadOnlyList<Employ> _taskTrackerAllEmploy;
    public ScheduleWindow(IReadOnlyList<Employ> taskTrackerAllEmploy, IEnumerable<string> resultStringAllEmploy, IEnumerable<Task> taskTrackerTaskToDistribution)
    {
        _taskTrackerAllEmploy = taskTrackerAllEmploy;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        InitializeComponent();
        EmployView.ItemsSource = resultStringAllEmploy;
        TasksView.ItemsSource = taskTrackerTaskToDistribution;
        EndLastTask.Content = "Time of all works: " + taskTrackerAllEmploy.MaxBy(x => x.EndLastTask).EndLastTask;
        return;

    }

    public void ExportDataGridToExcel(IReadOnlyList<Employ> allEmp, string filePath)
    {
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Result");
            int counter = 0;
            var rowOffset = 2;

            worksheet.Cells[1, 1].Value = "Worker ID";
            worksheet.Cells[1, 2].Value = "Task number";
            worksheet.Cells[1, 3].Value = "Time start";
            worksheet.Cells[1, 4].Value = "Time end";

            for (int i = 0; i < allEmp.Count; i++)
            {
                worksheet.Cells[i + rowOffset + counter, 1].Value = allEmp[i].Name;
                for (int j = 0; j < allEmp[i].WorkedTasks.Count; j++)
                {
                    worksheet.Cells[j + i + rowOffset + counter, 2].Value = allEmp[i].WorkedTasks[j].Name;
                    worksheet.Cells[j + i + rowOffset + counter, 3].Value = allEmp[i].WorkedTasks[j].StartWork;
                    worksheet.Cells[j + i + rowOffset + counter, 4].Value = allEmp[i].WorkedTasks[j].EndWork;
                }
                counter = allEmp[i].WorkedTasks.Count-1;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            FileInfo excelFile = new FileInfo(filePath);
            package.SaveAs(excelFile);

            //Автоматическое открытие файла после сохранения
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
        string fileName = $"ExportedData_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
        string filePath = Path.Combine(downloadsPath, fileName);

        ExportDataGridToExcel(_taskTrackerAllEmploy, filePath);

    }
}