using HR_System_Backend.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_System_Backend.Model.Input
{
    public class EmployeeInput
    {
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime? createdDate { get; set; }
        [StringLength(11)]
        public string mobile { get; set; }
        [StringLength(9)]
        public string phone { get; set; }
        public decimal? salary { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public int? allowCome { get; set; }
        public int? allowOut { get; set; }
        public int? baseTime { get; set; }
        public int? departmentId { get; set; }
        [Required]
        public int? categoryId { get; set; }
        [Required]
        public int? salaryId { get; set; }
        [Required]
        public int? shiftId { get; set; }
        public Week holiday { get; set; }
        public Week workDays { get; set; }
    }
}
