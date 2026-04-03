using EStudy.Communication.Responses.Errors;
using EStudy.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EStudy.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is EStudyException eStudyException)
            HandleProjectException(eStudyException, context);
        else
            ThrowUnknownException(context);
    }

    private static void HandleProjectException(EStudyException eStudyException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)eStudyException.GetStatusCode();

        var acceptLanguage = context.HttpContext.Request.Headers.AcceptLanguage.ToString();
        var details = eStudyException.GetErrors().Select(error => new ResponseErrorDetailJson
        {
            Code = error.Code,
            Message = AppErrorCatalog.Resolve(error.Code, acceptLanguage, error.Message),
            Field = error.Field
        }).ToList();

        context.Result = new ObjectResult(new ResponseErrorJson(details));
    }

    private static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var acceptLanguage = context.HttpContext.Request.Headers.AcceptLanguage.ToString();
        var message = AppErrorCatalog.Resolve(
            AppErrorCodes.General.Unexpected,
            acceptLanguage,
            "An unexpected error occurred");

        context.Result = new ObjectResult(new ResponseErrorJson(
        [
            new ResponseErrorDetailJson
            {
                Code = AppErrorCodes.General.Unexpected,
                Message = message
            }
        ]));
    }
}