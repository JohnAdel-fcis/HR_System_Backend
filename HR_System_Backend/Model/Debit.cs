using System;
using System.Collections.Generic;

#nullable disable

namespace HR_System_Backend.Model
{
    public partial class Debit
    {
        public Debit()
        {
            DebitTransactions = new HashSet<DebitTransaction>();
        }

        public int DebitId { get; set; }
        public int? EmployeeId { get; set; }
        public string DebitName { get; set; }
        public double? DebitAmount { get; set; }
        public double? RemainingDebitAmount { get; set; }
        public double? Installment { get; set; }
        public double? InstallmentPaidAmount { get; set; }
        public DateTime? LastInstallmentPayDate { get; set; }
        public string Notes { get; set; }
        public double? PaidAmount { get; set; }
        public DateTime? DebitDate { get; set; }
        public bool? Finished { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<DebitTransaction> DebitTransactions { get; set; }
    }
}
