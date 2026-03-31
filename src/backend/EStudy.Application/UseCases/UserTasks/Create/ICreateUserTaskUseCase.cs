using EStudy.Communication.Requests.UserTasks;
using EStudy.Communication.Responses.UserTasks;

namespace EStudy.Application.UseCases.UserTasks.Create;

public interface ICreateUserTaskUseCase
{
    Task<ResponseUserTaskJson> Execute(RequestCreateUserTaskJson request);
}

