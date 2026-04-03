using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class UnauthorizedException : EStudyException
{
    public UnauthorizedException(string message, string code = AppErrorCodes.General.Unauthorized) : base(message, code)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}