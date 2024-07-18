using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeManagement1
{
    public class RoleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CEOTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var employee = item as Employee;
            if (employee != null && employee.CompanyRole == Role.CEO)
            {
                return CEOTemplate;
            }
            return DefaultTemplate;
        }
    }
}
