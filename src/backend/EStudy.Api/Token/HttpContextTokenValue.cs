using EStudy.Domain.Security.Tokens;

namespace EStudy.Api.Token;

public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    public string ValueToken()
    {
        var authorization = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        
        return authorization["Bearer ".Length..].Trim();
    }
}