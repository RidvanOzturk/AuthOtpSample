namespace AuthOtpSample.Api.Models.Request;

public record UpdateProfileRequest(string? Name, string? Surname, DateOnly? DateOfBirth);
