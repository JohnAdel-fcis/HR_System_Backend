using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class ShiftInput
    {
        [Required]
        public string shiftName { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        [Required]
        public string timeFrom { get; set; }
        [Required]
        public string timeTo { get; set; }
    }
}
