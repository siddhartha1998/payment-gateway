using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Application.Users.Commands;

namespace PaymentGateway.Server.Infrastructure.Identity
{

    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
   
    

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

        public async Task<ApplicationUser> GetUserByIdAsync(int userId)
        {
            ApplicationUser result = await _userManager.FindByIdAsync(userId.ToString());
            return result;
        }

        public Task<string> GetUserNameAsync(int userId)
        {
            throw new NotImplementedException();
        }

    }
}