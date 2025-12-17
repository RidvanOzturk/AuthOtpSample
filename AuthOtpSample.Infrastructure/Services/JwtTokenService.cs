using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthOtpSample.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthOtpSample.Infrastructure.Services;

public class JwtTokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(int userId, string email)
    {
        var jwt = config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
        };


        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
