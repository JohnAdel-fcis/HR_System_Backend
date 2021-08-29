using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model.Helper;

namespace HR_System_Backend.Model.Input
{
    public class ItemTransactionInput
    {
        public ItemTransactionInput()
        {
            Transactions = new List<ItemTransactionViewModel> () ;
        }
        public int EmployeeId { get; set; }
        public List<ItemTransactionViewModel> Transactions {get;set;}
    }
    
}
