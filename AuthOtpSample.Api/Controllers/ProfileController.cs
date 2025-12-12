using AuthOtpSample.Api.Contracts.Profile;
using AuthOtpSample.Application.Features.Profile;
using AuthOtpSample.Application.Services;
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
        var cmd = new UpdateProfileCommand(request.FirstName, request.LastName, request.DateOfBirth);
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
