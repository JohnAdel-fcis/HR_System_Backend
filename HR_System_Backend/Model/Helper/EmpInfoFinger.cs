using System;

namespace HR_System_Backend.Model.Helper
{
    public class EmpInfoFinger
    {
        public int idwEnrollNumber { get; set; }
        public string name { get; set; }
        public int? idwVerifyMode { get; set; }
        public int? idwInOutMode { get; set; }
        public DateTime LogDate { get; set; }
        public string LogTime { get; set; }
        public int? deviceId { get; set; }
    }
}