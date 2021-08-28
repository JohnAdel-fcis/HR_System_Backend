using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class ItemTransactionInput
    {
        public int ItemId { get; set; }
        public int EmployeeId { get; set; }

        public int? Quantity { get; set; }

        public DateTime? Date { get; set; }



    }
}
