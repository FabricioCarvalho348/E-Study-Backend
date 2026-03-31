using AutoMapper;
using EStudy.Communication.Responses.UserTasks;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Domain.Services.LoggedUser;

namespace EStudy.Application.UseCases.UserTasks.GetAll;

public class GetAllUserTasksUseCase(
    ILoggedUser loggedUser,
    IUserTaskRepository userTaskRepository,
    IMapper mapper) : IGetAllUserTasksUseCase
{
    public async Task<List<ResponseUserTaskJson>> Execute()
    {
        var user = await loggedUser.User();
        var tasks = await userTaskRepository.GetAllByUserId(user.Id);

        return mapper.Map<List<ResponseUserTaskJson>>(tasks);
    }
}
