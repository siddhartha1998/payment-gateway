﻿using Microsoft.AspNetCore.Identity;
using PaymentGateway.Server.Infrastructure.Identity;

namespace PaymentGateway.Server.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var administratorRole = new ApplicationRole { Name = "SuperAdmin", Description = "This is the super admin" };

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var adminstratorUser = new ApplicationUser { UserName = "admin", Email = "admin@gmail.com",PhoneNumber = "0123456789",FullName = "Siddhiganesh Joshi", IsActive = true };

            if (userManager.Users.All(u => u.UserName != adminstratorUser.UserName))
            {
                await userManager.CreateAsync(adminstratorUser, "Passw0rd@123");
                await userManager.AddToRoleAsync(adminstratorUser, administratorRole.Name);
            }
        }
    }
}
