using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class InvalidLoginException : EStudyException
{
    public InvalidLoginException() : base("Invalid email or password.")
    {
    }

    public override IList<string> GetErrorMessages() => [Message];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}