using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Response
{
    public class DeviceResponse
    {
        public int DeviceId { get; set; }
        public string DeviceIp { get; set; }
        public string DevicePort { get; set; }
    }
}
