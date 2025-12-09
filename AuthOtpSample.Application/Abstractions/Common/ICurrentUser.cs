namespace AuthOtpSample.Application.Abstractions.Common;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? Email { get; }
}