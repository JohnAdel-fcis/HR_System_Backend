using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class BounseDiscount
    {
        public int BdId { get; set; }
        public double? Amount { get; set; }
        public bool? Bonuse { get; set; }
        public bool? Discount { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
