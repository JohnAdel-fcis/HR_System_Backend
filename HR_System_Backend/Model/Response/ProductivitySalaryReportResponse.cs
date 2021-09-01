using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class ProductivitySalaryReportResponse
    {
        public int EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? ItemsNum { get; set; }
        public double? Bonus { get; set; }
        public double? Descount { get; set; }
        public double? Installment { get; set; }
        public double? Salary { get; set; }
        
    }
}
