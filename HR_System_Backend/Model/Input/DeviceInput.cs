using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class DeviceInput
    {
        public string deviceIP { get; set; }
        public string devicePort { get; set; }
        public int? priority { get; set; }
        public string deviceName { get; set; }
        public int?  branchId { get; set; }
    }
}
