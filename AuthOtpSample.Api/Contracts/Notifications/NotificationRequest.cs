namespace AuthOtpSample.Api.Contracts.Notifications;

public record NotificationRequest(bool IsEmailNotificationEnabled, bool IsSmsNotificationEnabled);
