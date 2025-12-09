using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthOtpSample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    public IActionResult Login()
    {
        return Ok();
    }
}
