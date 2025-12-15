using AuthOtpSample.Api.Services;
using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Services.Contracts;
using AuthOtpSample.Application.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthOtpSample.Api.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices(IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<INotificationService, NotificationService>();


            var key = config["Jwt:Key"];

            if (!string.IsNullOrWhiteSpace(key))
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                        };
                    });
            }
            else
            {
                services.AddAuthentication();
            }

            services.AddAuthorization();

            return services;
        }
    }
}
