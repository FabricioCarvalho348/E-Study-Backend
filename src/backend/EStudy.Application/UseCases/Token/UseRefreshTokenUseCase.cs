using EStudy.Communication.Requests.Tokens;
using EStudy.Communication.Responses.Tokens;

namespace EStudy.Application.UseCases.Token;

public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    public Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        throw new NotImplementedException();
    }
}