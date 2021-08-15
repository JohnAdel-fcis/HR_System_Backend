using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Covenant
    {
        public int CovenantId { get; set; }
        public string CovenantPath { get; set; }
        public string CovenantName { get; set; }
        public DateTime? CovenantFromDate { get; set; }
        public DateTime? CovenantToDate { get; set; }
        public int? EmplyeeId { get; set; }

        public virtual Employee Emplyee { get; set; }
    }
}
