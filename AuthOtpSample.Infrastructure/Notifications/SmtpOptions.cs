namespace AuthOtpSample.Infrastructure.Notifications;

public sealed class SmtpOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FromName { get; init; } = "AuthOtpSample";
    public string FromEmail { get; init; } = string.Empty;
    public bool UseSsl { get; init; } = false;
    public bool UseStartTls { get; init; } = true;
}
