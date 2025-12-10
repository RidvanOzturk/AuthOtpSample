using AuthOtpSample.Api.Contracts.Account;
using AuthOtpSample.Application.Features.Account.ConfirmOtp;
using AuthOtpSample.Application.Features.Account.Register;
using AuthOtpSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService account) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await account.RegisterAsync(new RegisterCommand(request.Email, request.Password), cancellationToken);
        return Ok();
    }

    [HttpPost("Confirm-otp")]
    public async Task<IActionResult> ConfirmOtp([FromBody] ConfirmOtpRequest request, CancellationToken cancellationToken)
    {
        await account.ConfirmOtpAsync(new ConfirmOtpCommand(request.Email, request.Otp), cancellationToken);
        return Ok();
    }
}
