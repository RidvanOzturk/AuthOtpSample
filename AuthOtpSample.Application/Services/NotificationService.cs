using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Features.Notifications;
using AuthOtpSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Application.Services;

public class NotificationService(IAppDbContext appDbContext, ICurrentUser currentUser) : INotificationService
{
    private int GetUserIdOrThrow()
    {
        return currentUser.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
    }

    public async Task<NotificationDto> GetAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var entity = await appDbContext.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.UserId == userId, cancellationToken);

        if (entity is null)
        {
            return new NotificationDto(false, false);
        }

        return new NotificationDto(entity.IsEmailNotificationEnabled, entity.IsSmsNotificationEnabled);
    }

    public async Task UpsertAsync(UpdateNotificationCommand command, CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var entity = await appDbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, cancellationToken);

        if (entity is null)
        {
            entity = new Notification
            {
                UserId = userId,
                IsEmailNotificationEnabled = command.IsEmailNotificationEnabled,
                IsSmsNotificationEnabled = command.IsSmsNotificationEnabled
            };
            appDbContext.Notifications.Add(entity);
        }
        else
        {
            entity.IsEmailNotificationEnabled = command.IsEmailNotificationEnabled;
            entity.IsSmsNotificationEnabled = command.IsSmsNotificationEnabled;
        }

        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        var userId = GetUserIdOrThrow();

        var entity = await appDbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, cancellationToken);

        if (entity is null)
        {
            return;
        }

        appDbContext.Notifications.Remove(entity);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
