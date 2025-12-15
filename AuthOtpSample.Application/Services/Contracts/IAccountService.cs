using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface IAccountService
{
    Task RegisterAsync(RegisterDto command, CancellationToken cancellationToken);
    Task ConfirmOtpAsync(ConfirmOtpDto command, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(ForgotPasswordDto command, CancellationToken cancellationToken);
    Task ConfirmPasswordOtpAsync(ConfirmPasswordOtpDto command, CancellationToken cancellationToken);
    Task DeleteAccountAsync(int userId, CancellationToken cancellationToken);
}
