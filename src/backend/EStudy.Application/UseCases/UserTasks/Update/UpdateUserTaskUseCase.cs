using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.Update;

public class UpdateUserTaskUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IUnitOfWork unitOfWork) : IUpdateUserTaskUseCase
{
    public async Task Execute(long taskId, RequestUpdateUserTaskJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var userTask = await userTaskRepository.GetById(taskId, user.Id);

        if (userTask is null)
            throw new NotFoundException("Tarefa nao encontrada.");

        userTask.Title = request.Title;
        userTask.Description = request.Description;
        userTask.DueDate = request.DueDate;
        userTask.IsCompleted = request.IsCompleted;

        userTaskRepository.Update(userTask);
        await unitOfWork.Commit();
    }

    private static async Task Validate(RequestUpdateUserTaskJson request)
    {
        var validator = new UpdateUserTaskValidator();
        var result = await validator.ValidateAsync(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.ToAppErrors());
    }
}
