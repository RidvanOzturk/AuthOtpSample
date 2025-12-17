using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface IAccountService
{
    Task RegisterAsync(RegisterDto request, CancellationToken cancellationToken);
    Task ConfirmOtpAsync(ConfirmOtpDto request, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(ForgotPasswordDto request, CancellationToken cancellationToken);
    Task ConfirmPasswordOtpAsync(ConfirmPasswordOtpDto request, CancellationToken cancellationToken);
    Task DeleteAccountAsync(int userId, CancellationToken cancellationToken);
}
