using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthOtpSample.Infrastructure.Services;

public sealed class NotificationBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var smsSender = scope.ServiceProvider.GetRequiredService<ISmsSender>();

                var users = await appDbContext.Users
                    .AsNoTracking()
                    .Where(u => u.IsActive && u.Notification != null)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.Name,
                        u.Surname,
                        u.Notification!.IsEmailNotificationEnabled,
                        u.Notification!.IsSmsNotificationEnabled
                    })
                    .Where(x => x.IsEmailNotificationEnabled || x.IsSmsNotificationEnabled)
                    .ToListAsync(cancellationToken);


                foreach (var user in users)
                {
                    var userName = string.IsNullOrWhiteSpace(user.Name) && string.IsNullOrWhiteSpace(user.Surname)
                        ? user.Email
                        : $"{user.Name} {user.Surname}".Trim();

                    var message = $"Merhaba {userName}, uygulamamızı kullandığınız için teşekkür ederiz.";

                    if (user.IsEmailNotificationEnabled)
                    {
                        await emailSender.SendAsync(
                            user.Email,
                            "Teşekkürler",
                            message,
                            cancellationToken);
                    }

                    if (user.IsSmsNotificationEnabled)
                    {
                        await smsSender.SendAsync(user.Id, message, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while running NotificationBackgroundService loop");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}