using EStudy.Communication.Responses.Errors;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Api.Filters;

public class AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository repository)
    : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await repository.ExistActiveUserWithIdentifier(userIdentifier);
            if (exist.IsFalse())
            {
                throw new UnauthorizedException(
                    AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.UserNotFound),
                    AppErrorCodes.Auth.UserNotFound);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            var message = ResolveMessage(context, AppErrorCodes.Auth.TokenExpired, "TokenIsExpired");
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(
            [
                new ResponseErrorDetailJson
                {
                    Code = AppErrorCodes.Auth.TokenExpired,
                    Message = message
                }
            ])
            {
                TokenIsExpired = true,
            });
        }
        catch (EStudyException eStudyException)
        {
            context.HttpContext.Response.StatusCode = (int)eStudyException.GetStatusCode();

            var details = eStudyException.GetErrors().Select(error => new ResponseErrorDetailJson
            {
                Code = error.Code,
                Message = ResolveMessage(context, error.Code, error.Message),
                Field = error.Field
            }).ToList();

            context.Result = new ObjectResult(new ResponseErrorJson(details));
        }
        catch
        {
            var message = ResolveMessage(context, AppErrorCodes.General.Unauthorized, "Unauthorized");
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(
            [
                new ResponseErrorDetailJson
                {
                    Code = AppErrorCodes.General.Unauthorized,
                    Message = message
                }
            ]));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new UnauthorizedException(
                AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.AuthorizationHeaderMissing),
                AppErrorCodes.Auth.AuthorizationHeaderMissing);
        }

        return authentication["Bearer ".Length..].Trim();
    }

    private static string ResolveMessage(AuthorizationFilterContext context, string code, string fallback)
    {
        var acceptLanguage = context.HttpContext.Request.Headers.AcceptLanguage.ToString();
        return AppErrorCatalog.Resolve(code, acceptLanguage, fallback);
    }
}