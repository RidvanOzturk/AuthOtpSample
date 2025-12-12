namespace AuthOtpSample.Application.Features.Notifications;

public record UpdateNotificationCommand(bool IsEmailNotificationEnabled, bool IsSmsNotificationEnabled);
