using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Application.Users.Commands;

namespace PaymentGateway.Server.Infrastructure.Identity
{

    public class IdentityService(UserManager<ApplicationUser> userManager,
                           IApplicationDbContext context)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IApplicationDbContext _context = context;
    

    public async Task<Result> CreateUserAsync(CreateUserCommand command)
        {
            IdentityResult identityResult = IdentityResult.Failed();

            var user = new ApplicationUser
            {
                UserName = command.UserName,
                Email = command.Email,
                IsActive = true
            };

            IdentityResult userCreateResult = await _userManager.CreateAsync(user, command.Password);
            return identityResult.ToApplicationResult();
        }

    }
}