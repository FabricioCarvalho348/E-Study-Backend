using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class RefreshTokenExpiredException : EStudyException
{
    public RefreshTokenExpiredException() : base("Refresh token has expired.")
    {
    }

    public override IList<string> GetErrorMessages() => [Message];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}