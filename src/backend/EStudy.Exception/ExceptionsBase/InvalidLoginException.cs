using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class InvalidLoginException : EStudyException
{
    public InvalidLoginException() : base(AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.InvalidCredentials), AppErrorCodes.Auth.InvalidCredentials)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}