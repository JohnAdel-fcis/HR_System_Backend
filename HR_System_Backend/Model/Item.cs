using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Item
    {
        public Item()
        {
            ItemTransactions = new HashSet<ItemTransaction>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double? ItemPrice { get; set; }
        public double? ItemCommission { get; set; }
        public int? ItemQnty { get; set; }
        public int? EmpId { get; set; }

        public virtual Employee Emp { get; set; }
        public virtual ICollection<ItemTransaction> ItemTransactions { get; set; }
    }
}
