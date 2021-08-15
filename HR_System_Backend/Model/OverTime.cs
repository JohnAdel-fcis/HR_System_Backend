using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class OverTime
    {
        public OverTime()
        {
            WorkTimes = new HashSet<WorkTime>();
        }

        public int OverTimeId { get; set; }
        public DateTime? OverTimeDate { get; set; }
        public int? OverTimeHours { get; set; }
        public double? OverHourPrice { get; set; }
        public double? OverTimePercentage { get; set; }
        public double? OverTimeTotal { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<WorkTime> WorkTimes { get; set; }
    }
}
