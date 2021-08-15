using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class DebitTransaction
    {
        public int TranId { get; set; }
        public int? DebitId { get; set; }
        public double? RemainingDebitAmount { get; set; }
        public double? Instalment { get; set; }
        public double? InstallmentPaidAmount { get; set; }
        public string Notes { get; set; }
        public DateTime? LastInstallmentPayDate { get; set; }
        public double? PaidAmount { get; set; }
        public DateTime? TranDate { get; set; }
        public bool? IsInstalment { get; set; }
        public double? PartialPayment { get; set; }

        public virtual Debit Debit { get; set; }
    }
}
