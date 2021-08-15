using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Shift
    {
        public Shift()
        {
            Employees = new HashSet<Employee>();
        }

        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
