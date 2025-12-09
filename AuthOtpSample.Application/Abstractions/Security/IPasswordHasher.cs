namespace AuthOtpSample.Application.Abstractions.Security;

public interface IPasswordHasher
{
    string Hash(string plaintextPassword);
    bool Verify(string plaintextPassword, string hashedPassword);
}
