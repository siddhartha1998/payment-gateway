using Microsoft.EntityFrameworkCore;
using PaymentGateway.Server.Domain.Entities;

namespace PaymentGateway.Server.Application.Common.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<Transaction> Transactions { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
