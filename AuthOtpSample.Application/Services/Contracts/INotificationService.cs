using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface INotificationService
{
    Task<NotificationDto> GetAsync(CancellationToken cancellationToken);
    Task UpsertAsync(UpdateNotificationDto request, CancellationToken cancellationToken);
    Task ResetAsync(CancellationToken cancellationToken);
}
