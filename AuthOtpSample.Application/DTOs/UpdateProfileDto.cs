namespace AuthOtpSample.Application.DTOs;

public record UpdateProfileDto(string? Name, string? Surname, DateOnly? DateOfBirth);
