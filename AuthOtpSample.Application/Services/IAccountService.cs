using AuthOtpSample.Application.Features.Account.ConfirmOtp;
using AuthOtpSample.Application.Features.Account.Register;

namespace AuthOtpSample.Application.Services;

public interface IAccountService
{
    Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken);
    Task ConfirmOtpAsync(ConfirmOtpCommand command, CancellationToken cancellationToken);
}
