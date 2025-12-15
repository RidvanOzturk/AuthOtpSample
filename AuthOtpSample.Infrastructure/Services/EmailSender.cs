using AuthOtpSample.Application.Abstractions.Notifications;
using AuthOtpSample.Infrastructure.Notifications;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace AuthOtpSample.Infrastructure.Services;

public sealed class EmailSender(
    IOptions<SmtpOptions> options,
    ILogger<EmailSender> logger
) : IEmailSender
{
    private readonly SmtpOptions _options = options.Value;

    public async Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Host) ||
            string.IsNullOrWhiteSpace(_options.User) ||
            string.IsNullOrWhiteSpace(_options.Password))
        {
            logger.LogWarning("SMTP configuration is missing. Email not sent.");
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Plain) { Text = body };

        using var client = new SmtpClient();

        try
        {
            var secureOption = _options.UseStartTls
                ? SecureSocketOptions.StartTls
                : (_options.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);

            await client.ConnectAsync(_options.Host, _options.Port, secureOption, cancellationToken);

            await client.AuthenticateAsync(_options.User, _options.Password, cancellationToken);

            await client.SendAsync(message, cancellationToken);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error while sending email to {Email}", toEmail);
        }

        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
