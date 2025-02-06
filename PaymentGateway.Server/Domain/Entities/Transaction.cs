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
        public long CardNumber { get; set; }
        public int Cvv {  get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
