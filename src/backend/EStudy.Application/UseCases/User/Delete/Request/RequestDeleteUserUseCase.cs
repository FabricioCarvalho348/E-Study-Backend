using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Services.LoggedUser;

namespace EStudy.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase(
    IUserRepository userUpdateRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork)
    : IRequestDeleteUserUseCase
{
    public async Task Execute()
    {
        var loggedUser1 = await loggedUser.User();

        var user = await userUpdateRepository.GetById(loggedUser1.Id);

        user.Active = false;
        userUpdateRepository.Update(user);

        await unitOfWork.Commit();
    }
}