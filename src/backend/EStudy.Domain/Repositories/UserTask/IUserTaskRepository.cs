namespace EStudy.Domain.Repositories.UserTask;

public interface IUserTaskRepository
{
    Task Add(Entities.UserTask userTask);
    Task<List<Entities.UserTask>> GetAllByUserId(long userId);
    Task<Entities.UserTask?> GetById(long id, long userId);
    void Delete(Entities.UserTask userTask);
    void Update(Entities.UserTask userTask);
}
