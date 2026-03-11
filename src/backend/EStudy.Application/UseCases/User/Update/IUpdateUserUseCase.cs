using EStudy.Communication.Requests.Users;

namespace EStudy.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}