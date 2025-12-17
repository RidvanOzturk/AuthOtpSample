using AuthOtpSample.Api.Models.Request;
using AuthOtpSample.Application.Abstractions.Common;
using AuthOtpSample.Application.Abstractions.Persistence;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace AuthOtpSample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService, IAppDbContext appDbContext) : ControllerBase
{
    [HttpPost("Register")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var exists = await appDbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (exists)
        {
            return Conflict();
        }
        await accountService.RegisterAsync(new RegisterDto(request.Email, request.Password), cancellationToken);
        return Ok();
    }

    [HttpPost("Confirm-otp")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ConfirmOtp([FromBody] ConfirmOtpRequest request, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        await accountService.ConfirmOtpAsync(new ConfirmOtpDto(request.Email, request.Otp), cancellationToken);
        return Ok();
    }

    [HttpPost("Forgot-Password")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await accountService.ForgotPasswordAsync(new ForgotPasswordDto(request.Email), cancellationToken);
        return Ok();
    }

    [HttpPost("Confirm-Password-otp")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ConfirmPasswordOtp([FromBody] ConfirmPasswordOtpRequest request, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        await accountService.ConfirmPasswordOtpAsync(
            new ConfirmPasswordOtpDto(request.Email, request.Otp, request.NewPassword),
            cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromServices] ICurrentUser currentUser, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var exists = await appDbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == userId, cancellationToken);

        if (!exists)
        {
            return NotFound();
        }

        await accountService.DeleteAccountAsync(userId, cancellationToken);
        return Ok();
    }
}
