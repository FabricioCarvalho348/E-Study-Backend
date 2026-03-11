using EStudy.Communication.Responses.Tokens;

namespace EStudy.Communication.Responses.Users;

public class ResponseRegisteredUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokensJson Tokens { get; set; } = default!;
}