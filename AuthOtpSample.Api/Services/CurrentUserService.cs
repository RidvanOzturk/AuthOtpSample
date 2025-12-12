using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthOtpSample.Application.Abstractions.Common;

namespace AuthOtpSample.Api.Services;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUser
{
    private HttpContext? HttpContext => accessor.HttpContext;

    public bool IsAuthenticated =>
        HttpContext?.User?.Identity?.IsAuthenticated == true;

    public int? UserId
    {
        get
        {
            var user = HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var raw =
                user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return int.TryParse(raw, out var id) ? id : (int?)null;
        }
    }

    public string? Email
    {
        get
        {
            var user = HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            return user.FindFirstValue(ClaimTypes.Email) ??
                   user.FindFirstValue(JwtRegisteredClaimNames.Email);
        }
    }
}
