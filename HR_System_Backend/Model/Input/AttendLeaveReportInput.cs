using System;

namespace HR_System_Backend.Model.Input
{
    public class AttendLeaveReportInput
    {
        public int EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
    
}