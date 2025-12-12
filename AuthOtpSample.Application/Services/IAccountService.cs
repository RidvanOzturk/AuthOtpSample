using AuthOtpSample.Application.Features.Account.ConfirmOtp;
using AuthOtpSample.Application.Features.Account.ConfirmPasswordOtp;
using AuthOtpSample.Application.Features.Account.ForgotPassword;
using AuthOtpSample.Application.Features.Account.Register;

namespace AuthOtpSample.Application.Services;

public interface IAccountService
{
    Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken);
    Task ConfirmOtpAsync(ConfirmOtpCommand command, CancellationToken cancellationToken);
    Task ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken);
    Task ConfirmPasswordOtpAsync(ConfirmPasswordOtpCommand command, CancellationToken cancellationToken);
    Task DeleteAccountAsync(int userId, CancellationToken cancellationToken);
}
