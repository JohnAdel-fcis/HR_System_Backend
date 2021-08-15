using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Category
    {
        public Category()
        {
            Employees = new HashSet<Employee>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
