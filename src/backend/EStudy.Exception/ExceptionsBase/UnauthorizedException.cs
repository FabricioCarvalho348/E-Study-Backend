using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class UnauthorizedException : EStudyException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
    
    public override IList<string> GetErrorMessages() => [Message];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}