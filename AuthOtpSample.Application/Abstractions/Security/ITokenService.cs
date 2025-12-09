namespace AuthOtpSample.Application.Abstractions.Security;

public interface ITokenService
{
    string CreateToken(int userId, string email);
}
