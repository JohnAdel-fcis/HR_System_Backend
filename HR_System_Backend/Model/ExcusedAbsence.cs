using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class ExcusedAbsence
    {
        public int Id { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public int? EmpId { get; set; }
        public string Excuse { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
