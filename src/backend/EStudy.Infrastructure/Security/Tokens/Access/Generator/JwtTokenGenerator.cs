using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EStudy.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Infrastructure.Security.Tokens.Access.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}