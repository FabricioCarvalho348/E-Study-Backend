using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class RefreshTokenNotFoundException : EStudyException
{
    public RefreshTokenNotFoundException()
        : base(AppErrorCatalog.GetDefaultMessage(AppErrorCodes.Auth.RefreshTokenNotFound), AppErrorCodes.Auth.RefreshTokenNotFound)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}