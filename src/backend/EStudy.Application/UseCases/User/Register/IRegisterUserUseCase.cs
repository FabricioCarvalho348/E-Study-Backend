using EStudy.Communication.Requests.Users;
using EStudy.Communication.Responses.Users;

namespace EStudy.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public  Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}