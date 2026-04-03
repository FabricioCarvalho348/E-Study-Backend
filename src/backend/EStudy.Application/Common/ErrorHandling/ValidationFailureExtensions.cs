using EStudy.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace EStudy.Application.Common.ErrorHandling;

public static class ValidationFailureExtensions
{
    public static IList<AppError> ToAppErrors(this IEnumerable<ValidationFailure> failures)
    {
        return failures.Select(failure => new AppError(
            ResolveCode(failure.ErrorCode),
            failure.ErrorMessage,
            string.IsNullOrWhiteSpace(failure.PropertyName) ? null : failure.PropertyName)).ToList();
    }

    private static string ResolveCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.EndsWith("Validator", StringComparison.OrdinalIgnoreCase))
            return AppErrorCodes.General.Validation;

        return code;
    }
}
