using EStudy.Communication.Requests.UserTasks;

namespace EStudy.Application.UseCases.UserTasks.Update;

public interface IUpdateUserTaskUseCase
{
    Task Execute(long taskId, RequestUpdateUserTaskJson request);
}

