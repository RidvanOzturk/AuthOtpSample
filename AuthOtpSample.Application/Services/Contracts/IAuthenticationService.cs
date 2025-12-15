using AuthOtpSample.Application.DTOs;

namespace AuthOtpSample.Application.Services.Contracts;

public interface IAuthenticationService
{
    Task<string> LoginAsync(LoginDto command, CancellationToken cancellationToken);
}
