namespace AuthOtpSample.Application.DTOs;

public sealed record ConfirmPasswordOtpDto(string Email, string Otp, string NewPassword);
