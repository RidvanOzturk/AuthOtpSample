namespace AuthOtpSample.Application.Abstractions.Token;

public interface ITokenService
{
    string CreateToken(int userId, string email);
}
