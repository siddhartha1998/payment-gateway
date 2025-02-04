using PaymentGateway.Server.Domain.Common;
using PaymentGateway.Server.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Server.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
