using AuthOtpSample.Application.Features.Auth.Login;

namespace AuthOtpSample.Application.Services;

public interface IAuthenticationService
{
    Task<string> LoginAsync(LoginCommand command, CancellationToken cancellationToken);
}
