using EStudy.Domain.Security.Tokens;
using EStudy.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}