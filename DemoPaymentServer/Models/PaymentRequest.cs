using DemoPaymentServer.Enums;

namespace DemoPaymentServer.Models
{
    public class PaymentRequest
    {
        public long InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string? CardNumber { get; set; }
        public string CardType { get; set; } = string.Empty;
        public int Cvv { get; set; }

    }

    public class PaymentResponse
    {
        public long InvoiceNo { get; set; }
        public TransactionStatus Status { get; set; }
        public string? Message { get; set; }
        public long ReferenceNo { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}