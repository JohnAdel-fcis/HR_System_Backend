using System;

namespace HR_System_Backend.Model.Response
{
    public class DebitTransactionResponse
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
    }
}