using System;
using System.ComponentModel.DataAnnotations;
namespace HR_System_Backend.Model.Input
{
    public class AttendLeaveReportInput
    {
        public int? EmployeeId { get; set; }
        [Required]
        public DateTime? From { get; set; }
        [Required]
        public DateTime? To { get; set; }
    }
    
}