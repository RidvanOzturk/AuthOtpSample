using AuthOtpSample.Api.Models.Request;
using AuthOtpSample.Application.DTOs;
using AuthOtpSample.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController(IProfileService profile) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await profile.GetAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [HttpPut]
    public async Task<IActionResult> Upsert([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var cmd = new UpdateProfileDto(request.Name, request.Surname, request.DateOfBirth);
        await profile.UpdateAsync(cmd, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        await profile.ClearAsync(cancellationToken);
        return Ok();
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            ok = true,
            user = User.Identity?.Name,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
