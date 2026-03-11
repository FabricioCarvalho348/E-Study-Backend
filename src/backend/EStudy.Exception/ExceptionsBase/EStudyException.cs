using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public abstract class EStudyException : SystemException
{
    protected EStudyException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}