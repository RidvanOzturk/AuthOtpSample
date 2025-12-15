namespace AuthOtpSample.Application.DTOs;

public record UpdateNotificationDto(bool IsEmailNotificationEnabled, bool IsSmsNotificationEnabled);
