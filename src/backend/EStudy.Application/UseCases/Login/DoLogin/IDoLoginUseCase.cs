using EStudy.Communication.Requests.DoLogin;
using EStudy.Communication.Responses.Users;

namespace EStudy.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}