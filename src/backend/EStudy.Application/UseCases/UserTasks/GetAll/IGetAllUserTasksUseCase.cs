using EStudy.Communication.Responses.UserTasks;

namespace EStudy.Application.UseCases.UserTasks.GetAll;

public interface IGetAllUserTasksUseCase
{
    Task<List<ResponseUserTaskJson>> Execute();
}

