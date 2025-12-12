namespace AuthOtpSample.Application.Features.Profile;

public record UpdateProfileCommand(string? Name, string? Surname, DateOnly? DateOfBirth);
