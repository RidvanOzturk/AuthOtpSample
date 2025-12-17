using AuthOtpSample.Api.Services;
using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Services.Contracts;
using AuthOtpSample.Application.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
// diğer using’ler...

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
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,

                            ValidateIssuer = false,
                            ValidateAudience = false
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

        public IServiceCollection AddApiRateLimiting()
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = (ctx, _) =>
                {
                    ctx.HttpContext.Response.Headers["Retry-After"] = "60";
                    ctx.HttpContext.Response.Headers["X-Trace-Id"] = ctx.HttpContext.TraceIdentifier;
                    return ValueTask.CompletedTask;
                };

                options.AddPolicy("auth", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    });
                });

                options.AddPolicy("api", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 120,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    });
                });
            });

            return services;
        }
    }
}
