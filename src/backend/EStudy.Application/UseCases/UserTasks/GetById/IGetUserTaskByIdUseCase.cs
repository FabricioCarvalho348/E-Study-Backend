using EStudy.Communication.Responses.UserTasks;

namespace EStudy.Application.UseCases.UserTasks.GetById;

public interface IGetUserTaskByIdUseCase
{
    Task<ResponseUserTaskJson> Execute(long taskId);
}

