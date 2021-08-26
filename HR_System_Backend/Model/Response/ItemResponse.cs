using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class ItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double? ItemPrice { get; set; }
        public double? ItemCommission { get; set; }
        public int? ItemQnty { get; set; }
    }
}
