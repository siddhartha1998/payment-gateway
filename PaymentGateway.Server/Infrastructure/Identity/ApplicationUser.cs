using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Domain.Entities;

namespace PaymentGateway.Server.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class ApplicationRole : IdentityRole<int> 
    {
        public string? Description { get; set; }
        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public ApplicationRole() { }
        public ApplicationRole(string roleName) : base(roleName)
        {

        }
        public ApplicationRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
        
    }
}
