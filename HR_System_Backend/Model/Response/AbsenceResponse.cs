using System;

namespace HR_System_Backend.Model.Response
{
    public class AbsenceResponse
    {
        public int? id { get; set; }
        public DateTime? date { get; set; }
        public int? empId { get; set; }
        public bool? excusedAbsence { get; set; }
        public string excuse { get; set; }
    }
}