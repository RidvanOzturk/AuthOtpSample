using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Abstractions.Token;
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
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAppDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ITokenService, JwtTokenService>();
            services.Configure<SmtpOptions>(config.GetSection("Smtp"));
            services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddScoped<ISmsSender, SmsSenderService>();

            services.AddHostedService<NotificationBackgroundService>();

            return services;
        }
    }
}
