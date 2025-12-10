namespace AuthOtpSample.Application.Features.Account.ConfirmOtp;

public record ConfirmOtpCommand(string Email, string Otp);
