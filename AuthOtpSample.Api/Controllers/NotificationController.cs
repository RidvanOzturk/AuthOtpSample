using AuthOtpSample.Api.Models.Request;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(INotificationService notifications) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetForCurrentUser(CancellationToken cancellationToken)
    {
        var result = await notifications.GetAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateForCurrentUser([FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var cmd = new UpdateNotificationDto(
            request.IsEmailNotificationEnabled,
            request.IsSmsNotificationEnabled);

        await notifications.UpsertAsync(cmd, cancellationToken);
        return Ok();
    }

    [HttpPut("users")]
    public async Task<IActionResult> UpdateForCurrentUser([FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var cmd = new UpdateNotificationDto(
            request.IsEmailNotificationEnabled,
            request.IsSmsNotificationEnabled);

        await notifications.UpsertAsync(cmd, cancellationToken);
        return Ok();
    }


    [HttpDelete("users")]
    public async Task<IActionResult> ResetForCurrentUser(CancellationToken cancellationToken)
    {
        await notifications.ResetAsync(cancellationToken);
        return Ok();
    }
}
