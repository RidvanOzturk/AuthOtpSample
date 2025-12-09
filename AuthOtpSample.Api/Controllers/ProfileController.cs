using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProfile()
    {
        return Ok();
    }
}
