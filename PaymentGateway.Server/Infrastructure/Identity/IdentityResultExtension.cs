using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Application.Common;

namespace PaymentGateway.Server.Infrastructure.Identity
{
    public static class IdentityResultExtension
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
