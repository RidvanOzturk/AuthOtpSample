using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.Abstractions.Security;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Application.Services.Implementations;

public class AuthenticationService(IAppDbContext appDbContext, IPasswordHasher hasher, ITokenService tokens)
    : IAuthenticationService
{
    public async Task<string> LoginAsync(LoginDto request, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is not active");
        }

        if (!hasher.Verify(request.Password, user.HashPassword))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return tokens.CreateToken(user.Id, user.Email);
    }
}
