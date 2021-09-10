using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class CovenantResponse
    {
        public int empId { get; set; }
        public string CovenantName { get; set; }
        public DateTime? CovenantFromDate { get; set; }
        public DateTime? CovenantToDate { get; set; }
        public int? EmplyeeId { get; set; }

    }
}
