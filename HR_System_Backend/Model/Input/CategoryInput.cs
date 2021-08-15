using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HR_System_Backend.Model.Input
{
    public class CategoryInput
    {
        [Required]
        public string categoryName { get; set; }
    }
}
