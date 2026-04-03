namespace EStudy.Communication.Responses.Errors;

public class ResponseErrorDetailJson
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Field { get; set; }
}

