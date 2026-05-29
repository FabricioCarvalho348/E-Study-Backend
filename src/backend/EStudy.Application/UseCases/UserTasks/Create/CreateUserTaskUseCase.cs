using AutoMapper;
using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Communication.Responses.UserTasks;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserCustomCategory;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.Create;

public class CreateUserTaskUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IUserCustomCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICreateUserTaskUseCase
{
    public async Task<ResponseUserTaskJson> Execute(RequestCreateUserTaskJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();
        var customCategoryId = request.CustomCategoryId is > 0 ? request.CustomCategoryId : null;

        await ValidateCustomCategory(customCategoryId, user.Id);

        var userTask = mapper.Map<Domain.Entities.UserTask>(request);
        userTask.UserId = user.Id;
        userTask.Category = request.Category ?? Domain.Entities.CategoryEnum.SemCategoria;
        userTask.CustomCategoryId = customCategoryId;

        await userTaskRepository.Add(userTask);
        await unitOfWork.Commit();

        return mapper.Map<ResponseUserTaskJson>(userTask);
    }

    private static async Task Validate(RequestCreateUserTaskJson request)
    {
        var validator = new CreateUserTaskValidator();
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
