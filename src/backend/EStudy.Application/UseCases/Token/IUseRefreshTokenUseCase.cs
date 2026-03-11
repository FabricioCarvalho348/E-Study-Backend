using EStudy.Communication.Requests.Tokens;
using EStudy.Communication.Responses.Tokens;

namespace EStudy.Application.UseCases.Token;

public interface IUseRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}