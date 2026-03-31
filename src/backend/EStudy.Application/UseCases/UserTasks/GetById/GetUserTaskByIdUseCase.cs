using AutoMapper;
using EStudy.Communication.Responses.UserTasks;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.UserTasks.GetById;

public class GetUserTaskByIdUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IMapper mapper) : IGetUserTaskByIdUseCase
{
    public async Task<ResponseUserTaskJson> Execute(long taskId)
    {
        var user = await loggedUser.User();
        var userTask = await userTaskRepository.GetById(taskId, user.Id);

        if (userTask is null)
            throw new NotFoundException("Tarefa nao encontrada.");

        return mapper.Map<ResponseUserTaskJson>(userTask);
    }
}
