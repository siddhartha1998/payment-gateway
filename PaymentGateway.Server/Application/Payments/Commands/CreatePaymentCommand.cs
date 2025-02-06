using Azure;
using MediatR;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Domain.Entities;
using PaymentGateway.Server.Domain.Enums;

namespace PaymentGateway.Server.Application.Payments.Commands
{
    public class CreatePaymentCommand : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string PayerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long CardNumber { get; set; }
        public string ExpiryDate { get; set; } = string.Empty;
        public int CVV { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreatePaymentCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
             var transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethod = request.PaymentMethod,
                Email = request.Email
                UserId = userId,
                Status = response.Status
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
        }
    }
}
