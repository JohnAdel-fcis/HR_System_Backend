using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class ItemTransactionResponse
    {
        public int TarnsId { get; set; }
        public DateTime? TransDate { get; set; }
        public int? ItemQuantity { get; set; }
        public double? ItemComissions { get; set; }
        public int? ItemId { get; set; }
        public int? EmpId { get; set; }
    }
}
