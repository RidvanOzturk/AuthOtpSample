using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Abstractions.Security;
using AuthOtpSample.Infrastructure.Database;
using AuthOtpSample.Infrastructure.Notifications;
using AuthOtpSample.Infrastructure.Security;
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

            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IEmailSender, ConsoleEmailSender>();
            services.AddScoped<ISmsSender, ConsoleSmsSender>();

            return services;
        }
    }
}
