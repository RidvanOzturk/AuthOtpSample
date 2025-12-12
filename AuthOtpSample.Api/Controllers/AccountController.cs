using AuthOtpSample.Api.Contracts.Account;
using AuthOtpSample.Application.Features.Account.ConfirmOtp;
using AuthOtpSample.Application.Features.Account.ConfirmPasswordOtp;
using AuthOtpSample.Application.Features.Account.ForgotPassword;
using AuthOtpSample.Application.Features.Account.Register;
using AuthOtpSample.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[ApiController]
[Route("Account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await accountService.RegisterAsync(new RegisterCommand(request.Email, request.Password), cancellationToken);
        return Ok();
    }

    [HttpPost("Confirm-otp")]
    public async Task<IActionResult> ConfirmOtp([FromBody] ConfirmOtpRequest request, CancellationToken cancellationToken)
    {
        await accountService.ConfirmOtpAsync(new ConfirmOtpCommand(request.Email, request.Otp), cancellationToken);
        return Ok();
    }

    [HttpPost("Forgot-Password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await accountService.ForgotPasswordAsync(new ForgotPasswordCommand(request.Email), cancellationToken);
        return Ok();
    }

    [HttpPost("Confirm-Password-otp")]
    public async Task<IActionResult> ConfirmPasswordOtp([FromBody] ConfirmPasswordOtpRequest request, CancellationToken cancellationToken)
    {
        await accountService.ConfirmPasswordOtpAsync(
            new ConfirmPasswordOtpCommand(request.Email, request.Otp, request.NewPassword),
            cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await accountService.DeleteAccountAsync(id, cancellationToken);
        return Ok();
    }
}
