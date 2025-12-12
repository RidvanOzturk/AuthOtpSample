namespace AuthOtpSample.Api.Contracts.Profile;

public record UpdateProfileRequest(string? FirstName, string? LastName, DateOnly? DateOfBirth);
