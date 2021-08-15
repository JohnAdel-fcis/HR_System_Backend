using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class DebitInput
    {
        public int? EmployeeId { get; set; }
        public string DebitName { get; set; }
        public double? DebitAmount { get; set; }
        public double? Installment { get; set; }
        public string Notes { get; set; }
        public double? PaidAmount { get; set; }
        public DateTime? DebitDate { get; set; }
       
    }
}
