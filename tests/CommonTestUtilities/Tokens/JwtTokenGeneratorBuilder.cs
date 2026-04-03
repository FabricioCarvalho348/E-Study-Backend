using EStudy.Domain.Security.Tokens;
using EStudy.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    private const string TestSigningKey = "W5BSn3m1+z/mIiBiwJ#1r]=BS^1t=nc5";

    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: TestSigningKey);
}