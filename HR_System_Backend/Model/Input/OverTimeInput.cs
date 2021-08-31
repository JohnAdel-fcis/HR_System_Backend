using System;

namespace HR_System_Backend.Model.Input
{
    public class OverTimeInput
    {
        public int empId { get; set; }
        public double? hours {get;set;}
        public DateTime? date { get; set; }
        public double? overHourPrice { get; set; }
        public double? overTimePercentage { get; set; }
        public string note { get; set; }
    }
}