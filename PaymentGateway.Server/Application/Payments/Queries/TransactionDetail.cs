namespace PaymentGateway.Server.Application.Payments.Queries
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public long ReferenceNo { get; set; }
        public string PayerName { get; set; } = string.Empty;
        public string PayerEmail { get; set;} = string.Empty;
        public string PayerPhone { get; set;} = string.Empty;
        public string PaymentMethod {  get; set; } = string.Empty;
        public string PaymentStatus {  get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }
}
