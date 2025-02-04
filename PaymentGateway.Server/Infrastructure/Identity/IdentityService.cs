using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;

namespace PaymentGateway.Server.Infrastructure.Identity
{

    public class IdentityService(UserManager<User> userManager,
                           IApplicationDbContext context)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IApplicationDbContext _context = context;
    }

    public async Task<Result> CreateUserAsync(int userId)
        {

        }
}
