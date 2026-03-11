using EStudy.Communication.Responses.Errors;
using EStudy.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EStudy.Api.Filters;

public class ExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is EStudyException eStudyException)
            HandleProjectException(eStudyException, context);
        else
            ThrowUnknownException(context);
    }

    private static void HandleProjectException(EStudyException eStudyException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)eStudyException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(eStudyException.GetErrorMessages()));
    }

    private static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson("An unexpected error occurred"));
    }
}