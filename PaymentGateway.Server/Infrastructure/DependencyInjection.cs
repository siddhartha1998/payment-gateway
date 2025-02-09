using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentGateway.Server.Api.Services;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Infrastructure.Identity;
using PaymentGateway.Server.Infrastructure.Persistence;
using PaymentGateway.Server.Infrastructure.Services;
using System.Reflection;
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
                            ValidIssuer = configuration["JwtSettings:Issuer"],
                            ValidateIssuer = true,
                            ValidAudience = configuration["JwtSettings:Audience"],
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            services.AddAuthorization();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            AddSwaggerConfiguration(services);
            var secretKey = configuration["HmacSettings:SecretKey"];
            services.AddSingleton(new HmacService(secretKey));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PaymentGatewayAPI",
                    Version = "v1",
                    Description = "To test API from Swagger",
                    Contact = new OpenApiContact
                    {
                        Name = "API Support",
                        Url = new Uri("https://www.api.com/support"),
                        Email = "er.siddhartha1998@gmail.com"
                    },
                    TermsOfService = new Uri("https://www.api.com/termsandservices"),
                });

                config.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

          //  services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
