using AuthOtpSample.Application.Features.Notifications;

namespace AuthOtpSample.Application.Services;

public interface INotificationService
{
    Task<NotificationDto> GetAsync(CancellationToken cancellationToken);
    Task UpsertAsync(UpdateNotificationCommand command, CancellationToken cancellationToken);
    Task ResetAsync(CancellationToken cancellationToken);
}
