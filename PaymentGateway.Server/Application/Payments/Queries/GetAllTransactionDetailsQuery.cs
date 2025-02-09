using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Server.Application.Common.Interface;

namespace PaymentGateway.Server.Application.Payments.Queries
{
    public class GetAllTransactionDetailsQuery : IRequest<List<TransactionDetail>>
    {
    }

    public class GetAllTransactionDetailsQueryHandler : IRequestHandler<GetAllTransactionDetailsQuery, List<TransactionDetail>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _userService;

        public GetAllTransactionDetailsQueryHandler(IApplicationDbContext dbContext,
                                                    ICurrentUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        public async Task<List<TransactionDetail>> Handle(GetAllTransactionDetailsQuery request, CancellationToken cancellationToken)
        {
            List<TransactionDetail> details = await _dbContext.Transactions
                                                             //   .Where(x => x.UserId.ToString().Equals(_userService.UserId))
                                                                .Select(x => new TransactionDetail
                                                                {
                                                                    AccountNumber = x.AccountNumber,
                                                                    Amount = x.Amount,
                                                                    TransactionDate = x.TransactionDateTime,
                                                                    BankName = x.BankName,
                                                                    CardNumber = x.CardNumber,
                                                                    ExpiryDate = x.ExpiryDate,
                                                                    Cvv = x.Cvv,
                                                                    ReferenceNo = x.ReferenceNo,
                                                                    PayerName = x.PayerName,
                                                                    PaymentMethod = x.PaymentMethod.ToString(),
                                                                    PaymentStatus = x.PaymentStatus.ToString()
                                                                })
                                                                .ToListAsync(cancellationToken);

            return details;
        }
    }
}
