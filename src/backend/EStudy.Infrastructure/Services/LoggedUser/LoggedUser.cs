using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EStudy.Domain.Entities;
using EStudy.Domain.Security.Tokens;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly EStudyDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    
    public LoggedUser(EStudyDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> User()
    {
        var token = _tokenProvider.ValueToken();
        
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}