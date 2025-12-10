namespace AuthOtpSample.Application.Abstractions.Notifications;

public interface IEmailSender
{
    Task SendAsync(string toEmail, string subject, string body, CancellationToken ct);
}
