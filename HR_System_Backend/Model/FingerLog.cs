using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class FingerLog
    {
        public int LogId { get; set; }
        public int? Code { get; set; }
        public int? InOut { get; set; }
        public DateTime? LogDate { get; set; }
        public TimeSpan? LogTime { get; set; }
        public int? EmpId { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
