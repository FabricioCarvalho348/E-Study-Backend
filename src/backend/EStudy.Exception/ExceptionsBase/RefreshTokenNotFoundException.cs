using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class RefreshTokenNotFoundException : EStudyException
{
    public RefreshTokenNotFoundException() : base("Refresh token not found")
    {
    }

    public override IList<string> GetErrorMessages() => [Message];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}