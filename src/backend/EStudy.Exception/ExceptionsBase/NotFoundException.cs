using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class NotFoundException : EStudyException
{
    public NotFoundException(string message, string code = AppErrorCodes.General.NotFound) : base(message, code)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}