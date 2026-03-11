using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EStudy.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Infrastructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;
    private readonly string _issuer;
    private readonly string _audience;
    
    public JwtTokenValidator(string signingKey, string issuer, string audience)
    {
        _signingKey = signingKey;
        _issuer = issuer;
        _audience = audience;
    }
    

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
        
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        return Guid.Parse(userIdentifier);
    }
}