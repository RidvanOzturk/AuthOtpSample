using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface INotificationService
{
    Task<NotificationDto> GetAsync(CancellationToken cancellationToken);
    Task UpsertAsync(UpdateNotificationDto command, CancellationToken cancellationToken);
    Task ResetAsync(CancellationToken cancellationToken);
}
