namespace AuthOtpSample.Api.Models.Request;

public record ConfirmOtpRequest(string Email, string Otp);
