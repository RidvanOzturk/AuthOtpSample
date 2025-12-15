using AuthOtpSample.Api.Models.Request;
using AuthOtpSample.Api.Models.Response;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService, IAppDbContext appDbContext) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Unauthorized("Invalid credentials");
        }

        if (!user.IsActive)
        {
            return Unauthorized("Account is not active");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.HashPassword);
        if (!isPasswordValid)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = await authenticationService.LoginAsync(new LoginDto(request.Email, request.Password), cancellationToken);
        return Ok(new LoginResponse(token));
    }
}