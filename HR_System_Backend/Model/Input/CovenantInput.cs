using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class CovenantInput
    {
        public string CovenantName { get; set; }
        public DateTime? CovenantFromDate { get; set; }
        public DateTime? CovenantToDate { get; set; }
        public int? EmplyeeId { get; set; }
    }
}
