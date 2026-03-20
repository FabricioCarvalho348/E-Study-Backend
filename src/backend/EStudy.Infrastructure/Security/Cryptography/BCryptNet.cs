using EStudy.Domain.Security.Cryptography;

namespace EStudy.Infrastructure.Security.Cryptography;

public class BCryptNet : IPasswordEncrypter
{
    public string Encrypt(string password)
    {        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
             
        return passwordHash;

    }

    public bool IsValid(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}