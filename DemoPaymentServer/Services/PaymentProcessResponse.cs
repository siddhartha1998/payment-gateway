using DemoPaymentServer.Enums;

namespace DemoPaymentServer.Services
{
    public class PaymentProcessResponse
    {
        public TransactionStatus Status { get; set; }
        public string? Message { get; set; }
        public long ReferenceNo { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
