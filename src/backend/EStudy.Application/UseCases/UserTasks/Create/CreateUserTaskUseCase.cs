using AutoMapper;
using EStudy.Application.Common.ErrorHandling;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Communication.Responses.UserTasks;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.Create;

public class CreateUserTaskUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICreateUserTaskUseCase
{
    public async Task<ResponseUserTaskJson> Execute(RequestCreateUserTaskJson request)
    {
        await Validate(request);

        var user = await loggedUser.User();

        var userTask = mapper.Map<Domain.Entities.UserTask>(request);
        userTask.UserId = user.Id;

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
}
