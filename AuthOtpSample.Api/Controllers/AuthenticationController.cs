using AuthOtpSample.Api.Contracts.Auth;
using AuthOtpSample.Application.Features.Auth.Login;
using AuthOtpSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await authenticationService.LoginAsync(new LoginCommand(request.Email, request.Password), cancellationToken);
        return Ok(new LoginResponse(token));
    }
}
