using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EStudy.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Infrastructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator(string signingKey) : JwtTokenHandler, IAccessTokenValidator
{

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(signingKey),
            ClockSkew = new TimeSpan(0),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
        
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        return Guid.Parse(userIdentifier);
    }
}