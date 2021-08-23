using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class ItemTransaction
    {
        public int TarnsId { get; set; }
        public DateTime? TransDate { get; set; }
        public int? ItemQuantity { get; set; }
        public double? ItemComissions { get; set; }
        public int? ItemId { get; set; }
        public int? EmpId { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual Item Item { get; set; }
    }
}
