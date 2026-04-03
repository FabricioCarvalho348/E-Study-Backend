namespace EStudy.Exception.ExceptionsBase;

public sealed class AppError
{
    public string Code { get; }
    public string Message { get; }
    public string? Field { get; }

    public AppError(string code, string message, string? field = null)
    {
        Code = code;
        Message = message;
        Field = field;
    }
}

