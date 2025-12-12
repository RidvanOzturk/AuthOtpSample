namespace AuthOtpSample.Application.Abstractions.Notifications;

public interface ISmsSender
{
    Task SendAsync(int userId, string message, CancellationToken cancellationToken);
}
