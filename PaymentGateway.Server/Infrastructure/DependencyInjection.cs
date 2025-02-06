using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Infrastructure.Identity;
using PaymentGateway.Server.Infrastructure.Persistence;
using PaymentGateway.Server.Infrastructure.Services;
using System.Text;

namespace PaymentGateway.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            String connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
                options.EnableSensitiveDataLogging(); // For development only
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // Register Identity
            services.AddIdentityCore<ApplicationUser>(Options =>
            {
                Options.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // configuration Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JwtSettings:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["JwtSettings:Audience"],
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            services.AddAuthorization();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
