using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime? UploadDate { get; set; }
        public int? EmployeeId { get; set; }
        public string AddedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
