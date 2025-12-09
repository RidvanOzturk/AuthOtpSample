using System.Text;
using AuthOtpSample.Api.Services;
using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthOtpSample.Api.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices(IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            var key = config["Jwt:Key"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];

            if (!string.IsNullOrWhiteSpace(key) &&
                !string.IsNullOrWhiteSpace(issuer) &&
                !string.IsNullOrWhiteSpace(audience))
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issuer,
                            ValidAudience = audience,
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
