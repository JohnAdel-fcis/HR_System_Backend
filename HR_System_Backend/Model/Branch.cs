using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Branch
    {
        public Branch()
        {
            Devices = new HashSet<Device>();
            Employees = new HashSet<Employee>();
        }

        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
