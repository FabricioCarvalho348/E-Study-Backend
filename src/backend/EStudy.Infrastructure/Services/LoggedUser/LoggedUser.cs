using System.Security.Claims;
using EStudy.Domain.Entities;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;
using EStudy.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.Services.LoggedUser;

public class LoggedUser(EStudyDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    : ILoggedUser
{
    public async Task<User> User()
    {
        var userIdentifierClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Sid)?.Value;

        if (string.IsNullOrWhiteSpace(userIdentifierClaim) || Guid.TryParse(userIdentifierClaim, out var userIdentifier) == false)
            throw new UnauthorizedException(
                AppErrorCatalog.GetDefaultMessage(AppErrorCodes.General.Unauthorized),
                AppErrorCodes.General.Unauthorized);

        var user = await dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Active && entity.UserIdentifier == userIdentifier);

        if (user is null)
            throw new UnauthorizedException(
                AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.UserNotFound),
                AppErrorCodes.Auth.UserNotFound);

        return user;
    }
}