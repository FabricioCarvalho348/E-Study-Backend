namespace EStudy.Application.UseCases.UserTasks.Delete;

public interface IDeleteUserTaskUseCase
{
    Task Execute(long taskId);
}

