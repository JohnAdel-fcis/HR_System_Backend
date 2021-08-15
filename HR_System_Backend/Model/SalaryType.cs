using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class SalaryType
    {
        public SalaryType()
        {
            Employees = new HashSet<Employee>();
        }

        public int SalaryTypeId { get; set; }
        public string SalaryTypeName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
