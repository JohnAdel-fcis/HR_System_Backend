using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class ShiftResponse
    {
        public int shiftId { get; set; }
        public string shiftName { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
    }
}
