using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class NotFoundException : EStudyException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}