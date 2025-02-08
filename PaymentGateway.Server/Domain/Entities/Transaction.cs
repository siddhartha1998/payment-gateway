using PaymentGateway.Server.Domain.Common;
using PaymentGateway.Server.Domain.Enums;
using PaymentGateway.Server.Infrastructure.Identity;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Server.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? PayerName { get; set; }

        // For Card Transfer
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Cvv {  get; set; }

        // For Bank Transfer
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public TransactionStatus PaymentStatus { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public long ReferenceNo { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
