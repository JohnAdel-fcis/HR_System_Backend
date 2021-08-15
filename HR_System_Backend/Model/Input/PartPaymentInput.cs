using System;

namespace HR_System_Backend.Model.Input
{
    public class PartPaymentInput
    {
        public int debitId { get; set; }
        public float paymentAmount { get; set; }
        public DateTime paymentDate { get; set; }
    }
}