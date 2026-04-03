using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public abstract class EStudyException : SystemException
{
    private readonly IList<AppError> _errors;

    protected EStudyException(string message, string code = AppErrorCodes.General.Unexpected)
        : this([new AppError(code, message)])
    {
    }

    protected EStudyException(IList<AppError> errors)
        : base(errors.FirstOrDefault()?.Message ?? string.Empty)
    {
        _errors = errors;
    }

    public virtual IList<AppError> GetErrors() => _errors;

    public virtual IList<string> GetErrorMessages() => _errors.Select(error => error.Message).ToList();

    public abstract HttpStatusCode GetStatusCode();
}