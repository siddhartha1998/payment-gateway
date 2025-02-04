using Microsoft.AspNetCore.Identity;

namespace PaymentGateway.Server.Infrastructure.Identity
{
    public class User : IdentityUser<int>
    {
        public bool isActive { get; set; }
    }
}
