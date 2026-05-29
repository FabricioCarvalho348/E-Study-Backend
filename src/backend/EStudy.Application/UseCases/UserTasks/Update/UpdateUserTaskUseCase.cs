using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.Update;

public class UpdateUserTaskUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IUserCustomCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IUpdateUserTaskUseCase
{
    public async Task Execute(long taskId, RequestUpdateUserTaskJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var customCategoryId = request.CustomCategoryId is > 0 ? request.CustomCategoryId : null;

        await ValidateCustomCategory(customCategoryId, user.Id);

        var userTask = await userTaskRepository.GetById(taskId, user.Id);

        if (userTask is null)
            throw new NotFoundException("Tarefa nao encontrada.");

        userTask.Title = request.Title;
        userTask.Description = request.Description;
        userTask.DueDate = request.DueDate;
        userTask.IsCompleted = request.IsCompleted;
        if (request.Category.HasValue)
            userTask.Category = request.Category;
        userTask.CustomCategoryId = customCategoryId;

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

    private async Task ValidateCustomCategory(long? customCategoryId, long userId)
    {
        if (customCategoryId.HasValue is false)
            return;

        var category = await categoryRepository.GetById(customCategoryId.Value, userId);
        if (category is null)
            throw new ErrorOnValidationException(["Categoria informada nao foi encontrada para o usuario."]);
    }
}
