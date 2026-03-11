using EStudy.Communication.Requests.ChangePassword;

namespace EStudy.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}