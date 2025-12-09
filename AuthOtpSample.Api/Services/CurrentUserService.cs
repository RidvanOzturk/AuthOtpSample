using System.Security.Claims;
using AuthOtpSample.Application.Abstractions.Common;
using Microsoft.AspNetCore.Http;

namespace AuthOtpSample.Api.Services;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public int? UserId =>
        int.TryParse(Principal?.FindFirstValue("userId"), out var id) ? id : (int?)null;

    public string? Email =>
        Principal?.FindFirstValue("email") ?? Principal?.FindFirstValue(ClaimTypes.Email);
}