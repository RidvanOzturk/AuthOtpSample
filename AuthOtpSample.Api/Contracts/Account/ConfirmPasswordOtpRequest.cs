namespace AuthOtpSample.Api.Contracts.Account;

public sealed record ConfirmPasswordOtpRequest(string Email, string Otp, string NewPassword);
