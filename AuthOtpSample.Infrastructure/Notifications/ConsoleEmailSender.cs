using AuthOtpSample.Application.Abstractions.Notifications;

namespace AuthOtpSample.Infrastructure.Notifications;

public class ConsoleEmailSender : IEmailSender
{
    public Task SendAsync(string toEmail, string subject, string body, CancellationToken ct)
    {
        Console.WriteLine($"[EMAIL] To={toEmail} Subject={subject} Body={body}");
        return Task.CompletedTask;
    }
}
