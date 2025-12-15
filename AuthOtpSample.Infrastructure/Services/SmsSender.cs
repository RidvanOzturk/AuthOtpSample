using AuthOtpSample.Application.Abstractions.Notifications;

namespace AuthOtpSample.Infrastructure.Services;

public class SmsSender : ISmsSender
{
    public Task SendAsync(int userId, string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[SMS] ToUserId={userId} Message={message}");
        return Task.CompletedTask;
    }
}
