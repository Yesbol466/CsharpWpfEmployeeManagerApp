using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeManagement1
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public event EventHandler<EmployeeEventArgs> EmployeeAdded;
        public AddEmployeeWindow()
        {
            InitializeComponent();
            BirthDatePicker.SelectedDate = DateTime.Today.AddYears(-30);
            MaleRadioButton.IsChecked = true;
            SalaryCurrencyComboBox.SelectedIndex = 0;
            CompanyRoleComboBox.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                BirthDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(BirthCountryTextBox.Text) ||
                !int.TryParse(SalaryTextBox.Text, out int salary) ||
                SalaryCurrencyComboBox.SelectedItem == null ||
                CompanyRoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields correctly.");
                return;
            }

            
            string sex = MaleRadioButton.IsChecked == true ? "Male" : "Female";

            
            Employee newEmployee = new Employee(
                FirstNameTextBox.Text,
                LastNameTextBox.Text,
                sex,
                (DateTime)BirthDatePicker.SelectedDate,
                BirthCountryTextBox.Text,
                salary,
                (Currency)SalaryCurrencyComboBox.SelectedItem,
                (Role)CompanyRoleComboBox.SelectedItem
            );

            
            EmployeeAdded?.Invoke(this, new EmployeeEventArgs(newEmployee));
            this.Close();

        }
    }
    public class EmployeeEventArgs : EventArgs
    {
        public Employee Employee { get; }

        public EmployeeEventArgs(Employee employee)
        {
            Employee = employee;
        }
    }
}
