using EStudy.Domain.Security.Cryptography;
using EStudy.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build() => new BCryptNet();
}