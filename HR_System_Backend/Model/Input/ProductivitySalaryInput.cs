using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class ProductivitySalaryInput
    {
        public int? EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
