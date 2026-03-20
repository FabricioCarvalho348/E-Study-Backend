using EStudy.Domain.Security.Tokens;

namespace EStudy.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}