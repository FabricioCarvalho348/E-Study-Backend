using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.Delete;

public class DeleteUserTaskUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IUnitOfWork unitOfWork) : IDeleteUserTaskUseCase
{
    public async Task Execute(long taskId)
    {
        var user = await loggedUser.User();
        var userTask = await userTaskRepository.GetById(taskId, user.Id);

        if (userTask is null)
            throw new NotFoundException("Tarefa nao encontrada.");

        userTaskRepository.Delete(userTask);

        await unitOfWork.Commit();
    }
}
