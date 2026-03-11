using EStudy.Communication.Responses.Errors;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Api.Filters;

public class AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserRepository repository)
    : IAsyncAuthorizationFilter
{
    private readonly IUserRepository _repository = repository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
            if (exist.IsFalse())
            {
                throw new UnauthorizedException("User not found");
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
            {
                TokenIsExpired = true,
            });
        }
        catch (EStudyException eStudyException)
        {
            context.HttpContext.Response.StatusCode = (int)eStudyException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(eStudyException.GetErrorMessages()));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Unauthorized"));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new UnauthorizedException("Authorization header is missing");
        }

        return authentication["Bearer ".Length..].Trim();
    }
}