using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Win32;
using System.Globalization;
using System.Collections.ObjectModel;

namespace EmployeeManagement1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CurrentFilePath { get; set;}
        public bool IsModified { get; set;}
        public ObservableCollection<Employee> Employees { get; set; }
        public MainWindow()
        {
            Employees = new ObservableCollection<Employee>();
            InitializeComponent();
            DataContext = this;
            this.Closing += MainWindow_Closing;

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(openFileDialog.FileName);
                    Employees.Clear();
                    for (int i = 1; i < lines.Length; ++i)
                    {
                        var parts = lines[i].Split(';');
                        Employee employee = new Employee(
                            parts[0],
                            parts[1],
                            parts[2],
                            DateTime.ParseExact(parts[3], "dd.MM.yyyy", null),
                            parts[4],
                            int.Parse(parts[5]),
                            (Currency)Enum.Parse(typeof(Currency), parts[6]),
                            (Role)Enum.Parse(typeof(Role), parts[7])
                        );
                        Employees.Add(employee);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                IsModified = false;
            }
        }



        private void MoveUp_click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                int index = Employees.IndexOf(selectedEmployee);
                if (index > 0)
                {
                    Employees.Move(index, index - 1);
                    EmployeesListBox.SelectedItem = selectedEmployee;
                }
            }
        }

        private void MoveDown_click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                int index = Employees.IndexOf(selectedEmployee);
                if (index < Employees.Count - 1)
                {
                    Employees.Move(index, index + 1);
                    EmployeesListBox.SelectedItem = selectedEmployee;
                }
            }
        }

        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files|*.csv"
            };
            if(saveFileDialog.ShowDialog() == true)
            {
                CurrentFilePath= saveFileDialog.FileName;
                SaveFile();
            }

        }
        private void SaveFile()
        {
            try
            {
                var savefile = new SaveFileDialog
                {
                    Filter = "CSV files|*.csv"
                };
                if (savefile.ShowDialog() == true)
                {
                    CurrentFilePath = savefile.FileName;
                }
                using(StreamWriter writer = new StreamWriter(CurrentFilePath))
                {
                    writer.WriteLine("FirstName;LastName;Sex;BirthDate;BirthCountry;Salary;SalaryCurrency;CompanyRole");
                    foreach (var employee in Employees)
                    {
                        writer.WriteLine($"{employee.FirstName};{employee.LastName};{employee.Sex};{employee.BirthDate:dd.MM.yyyy};{employee.BirthCountry};{employee.Salary};{employee.SalaryCurrency};{employee.CompanyRole}");
                    }
                }
                IsModified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured{ex.Message}");
            }
        }


   

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(CurrentFilePath))
            {
                SaveFile();
            }
            else
            {
                SaveFileAs_Click(sender, e);
            }
        }

        private void CloseFile_Click(object sender, RoutedEventArgs e)
        {
            if (IsModified)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            Application.Current.Shutdown();
        }
        private void MarkAsModified()
        {
            IsModified = true;
        }

        private void AddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.EmployeeAdded += (s, args) =>
            {
                Employees.Add(args.Employee);
                IsModified = true;
            };
            addEmployeeWindow.ShowDialog();
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeesListBox.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                Employees.Remove(selectedEmployee);
                MarkAsModified();
            }

        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsModified)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes before exiting?", "Confirm", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; 
                }
            }
        }
    }
}