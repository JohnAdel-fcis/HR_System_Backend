using System;

namespace HR_System_Backend.Model.Response
{
    public class OverTimeResponse
    {
       public int OverTimeId { get; set; }
        public DateTime? OverTimeDate { get; set; }
        public double? OverTimeHours { get; set; }
        public double? OverHourPrice { get; set; }
        public double? OverTimePercentage { get; set; }
        public double? OverTimeTotal { get; set; }
        public string Notes { get; set; }

 
    }
}