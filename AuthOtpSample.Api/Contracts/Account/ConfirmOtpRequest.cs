namespace AuthOtpSample.Api.Contracts.Account;

public record ConfirmOtpRequest(string Email, string Otp);
