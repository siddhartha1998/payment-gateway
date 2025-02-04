using PaymentGateway.Server.Application.Users.Commands;

namespace PaymentGateway.Server.Application.Common.Interface
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(int userId);
        Task<Result> CreateUserAsync(CreateUserCommand request);
    }
}
