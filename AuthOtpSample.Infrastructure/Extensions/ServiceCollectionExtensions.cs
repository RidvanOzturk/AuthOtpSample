using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Security;
using AuthOtpSample.Infrastructure.Database;
using AuthOtpSample.Infrastructure.Notifications;
using AuthOtpSample.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthOtpSample.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("SqlServer")));

            services.AddScoped<ITokenService, JwtTokenService>();
            services.Configure<SmtpOptions>(config.GetSection("Smtp"));
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISmsSender, SmsSender>();

            return services;
        }
    }
}
