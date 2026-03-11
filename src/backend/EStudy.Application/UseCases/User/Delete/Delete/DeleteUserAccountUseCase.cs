using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;

namespace EStudy.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase(
    IUserRepository repository,
    IUnitOfWork unitOfWork) : IDeleteUserAccountUseCase
{
    public async Task Execute(Guid userIdentifier)
    {
        await repository.DeleteAccount(userIdentifier);

        await unitOfWork.Commit();
    }
}