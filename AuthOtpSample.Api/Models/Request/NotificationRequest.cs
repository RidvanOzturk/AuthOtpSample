namespace AuthOtpSample.Api.Models.Request;

public record NotificationRequest(bool IsEmailNotificationEnabled, bool IsSmsNotificationEnabled);
