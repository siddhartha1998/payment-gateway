using MediatR;
using Newtonsoft.Json;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Domain.Enums;
using System.Text;

using Transaction = PaymentGateway.Server.Domain.Entities.Transaction;
using IPaymentPublisher = PaymentGateway.Server.Application.Common.Interface.IPaymentPublisher;
using PaymentGateway.Server.Application.Common.Extensions;
using PaymentGateway.Server.Infrastructure.Identity;

namespace PaymentGateway.Server.Application.Payments.Commands
{
    public class CreatePaymentCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        //public string Email { get; set; } = string.Empty;
        public string? CardNumber { get; set; }
        public string ExpiryDate { get; set; } = string.Empty;
        public int Cvv { get; set; }
        //public string? CardType { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IPaymentPublisher _paymentPublisher;

        public CreatePaymentCommandHandler(IApplicationDbContext dbContext,
                                           IIdentityService identityService,
                                           IPaymentPublisher paymentPublisher)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _paymentPublisher = paymentPublisher;
        }

        public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentRequest = new PaymentProcessRequest
            {
                InvoiceNo = 8.RandomNumber(),
                Amount = request.Amount,
                CardNumber = request.CardNumber,
                CardType = "Visa",
                Cvv = request.Cvv
            };

            string response = await _paymentPublisher.PublishPaymentRequest(paymentRequest);
            if (response == null)
            {
                return Result.Failure("Response is null from rabbitMQ");
            }
            PaymentProcessResponse paymentResponse = JsonConvert.DeserializeObject<PaymentProcessResponse>(response);

            ApplicationUser user = await _identityService.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure("User could not be null");
            }
            var transaction = new Transaction
            {
                Amount = request.Amount,
                PaymentStatus = paymentResponse.Status,
                ReferenceNo = paymentResponse.ReferenceNo,
                Currency = request.Currency,
                PaymentMethod = request.PaymentMethod,
                TransactionDateTime = paymentResponse.TransactionDateTime,
                PayerName = user.FullName,
                User = user
            };

            _dbContext.Transactions.Add(addTransaction(transaction, request));
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private Transaction addTransaction(Transaction transaction, CreatePaymentCommand command)
        {
            if (command.PaymentMethod.Equals(PaymentMethod.Card))
            {
                transaction.CardNumber = command.CardNumber;
                transaction.Cvv = command.Cvv.ToString();
                transaction.ExpiryDate = command.ExpiryDate;
            }
            else
            {
                transaction.AccountNumber = command.AccountNumber;
                transaction.BankName = command.BankName;
            }
            return transaction;
        }
    }
}
