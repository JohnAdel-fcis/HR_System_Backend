using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class WorkTime
    {
        public int WorkId { get; set; }
        public DateTime? WorkDate { get; set; }
        public TimeSpan? WorkStart { get; set; }
        public TimeSpan? WorkEnd { get; set; }
        public int? EmployeeId { get; set; }
        public int? OverTimeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual OverTime OverTime { get; set; }
    }
}
