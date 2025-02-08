using PaymentGateway.Server.Application.Users.Commands;
using PaymentGateway.Server.Infrastructure.Identity;

namespace PaymentGateway.Server.Application.Common.Interface
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(int userId);
        Task<ApplicationUser> GetUserByIdAsync(int userId);
        Task<Result> CreateUserAsync(CreateUserCommand request);
    }
}
