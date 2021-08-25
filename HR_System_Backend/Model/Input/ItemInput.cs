using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class ItemInput
    {
        public string ItemName { get; set; }
        public double? ItemPrice { get; set; }
        public double? ItemCommission { get; set; }
        public int? ItemQnty { get; set; }
    }
}
