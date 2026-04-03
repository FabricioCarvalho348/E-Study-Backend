using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;

namespace EStudy.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteUserAccountUseCase(
        IUserDeleteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userIdentifier)
    {
        await _repository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}