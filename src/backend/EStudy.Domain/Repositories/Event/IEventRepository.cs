namespace EStudy.Domain.Repositories.Event;

public interface IEventRepository
{
    Task Add(Entities.Event eventEntity);
    Task<Entities.Event?> GetById(long id, long userId);
    Task<List<Entities.Event>> GetByDateRange(long userId, DateTime startDate, DateTime endDate, string? type);
    void Update(Entities.Event eventEntity);
    void Delete(Entities.Event eventEntity);
}

