using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class RefreshTokenExpiredException : EStudyException
{
    public RefreshTokenExpiredException() : base(AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.RefreshTokenExpired), AppErrorCodes.Auth.RefreshTokenExpired)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}