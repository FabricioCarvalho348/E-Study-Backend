namespace EStudy.Communication.Responses.Errors;

public class ResponseErrorJson
{
    public IList<string> Errors { get; set; }

    public IList<ResponseErrorDetailJson> Details { get; set; }

    public bool TokenIsExpired { get; set; }

    public ResponseErrorJson(IList<string> errors)
    {
        Errors = errors;
        Details = errors.Select(message => new ResponseErrorDetailJson
        {
            Code = string.Empty,
            Message = message
        }).ToList();
    }

    public ResponseErrorJson(IList<ResponseErrorDetailJson> details)
    {
        Details = details;
        Errors = details.Select(detail => detail.Message).ToList();
    }

    public ResponseErrorJson(string error)
    {
        Errors = [error];
        Details =
        [
            new ResponseErrorDetailJson
            {
                Code = string.Empty,
                Message = error
            }
        ];
    }
}