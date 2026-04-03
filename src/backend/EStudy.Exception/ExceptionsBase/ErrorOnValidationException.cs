using System.Net;

namespace EStudy.Exception.ExceptionsBase;

public class ErrorOnValidationException : EStudyException
{
    public ErrorOnValidationException(IList<string> errorMessages)
        : this(errorMessages.Select(message => new AppError(AppErrorCodes.General.Validation, message)).ToList())
    {
    }

    public ErrorOnValidationException(IList<AppError> errors) : base(errors)
    {
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}