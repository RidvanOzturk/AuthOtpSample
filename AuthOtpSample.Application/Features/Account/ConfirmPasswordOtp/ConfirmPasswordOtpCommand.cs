namespace AuthOtpSample.Application.Features.Account.ConfirmPasswordOtp;

public sealed record ConfirmPasswordOtpCommand(string Email, string Otp, string NewPassword);
