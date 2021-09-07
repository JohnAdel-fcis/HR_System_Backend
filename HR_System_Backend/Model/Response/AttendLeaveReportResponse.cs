using System;

namespace HR_System_Backend.Model.Response
{
    public class AttendLeaveReportResponse
    {
        public int? code { get; set; }
        public string employeeName { get; set; }
        public DateTime? date { get; set; }
        public string attendTime { get; set; }
        public string  leaveTime { get; set; }
        public double? workHours { get; set; }
        public double? late { get; set; }
        public double? overTime { get; set; }
        public double? leaveEarly { get; set; }
        public string shiftName { get; set; }
    }
}