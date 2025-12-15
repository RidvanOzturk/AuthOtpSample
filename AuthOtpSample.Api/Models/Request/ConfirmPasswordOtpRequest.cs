namespace AuthOtpSample.Api.Models.Request;

public sealed record ConfirmPasswordOtpRequest(string Email, string Otp, string NewPassword);
