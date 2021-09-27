using System;
using System.ComponentModel.DataAnnotations;
namespace HR_System_Backend.Model.Input
{
    public class AbsenceInput
    {
         [Required]
        public DateTime? date { get; set; }
        public int? empId { get; set; }
        public bool? excusedAbsence { get; set; }
        public string excuse { get; set; }
    }
}