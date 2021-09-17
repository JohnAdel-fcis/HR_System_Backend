using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Device
    {
        public int DeviceId { get; set; }
        public string DeviceIp { get; set; }
        public string DevicePort { get; set; }
        public int? Priority { get; set; }
        public string DeviceName { get; set; }
        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}
