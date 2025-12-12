using AuthOtpSample.Application.Abstractions.Notifications;

namespace AuthOtpSample.Infrastructure.Notifications;

public class ConsoleSmsSender : ISmsSender
{
    public Task SendAsync(int userId, string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[SMS] ToUserId={userId} Message={message}");
        return Task.CompletedTask;
    }
}
