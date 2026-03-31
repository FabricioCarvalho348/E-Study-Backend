using EStudy.Domain.Repositories.Event;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess.Repositories;

public class EventRepository(EStudyDbContext dbContext) : IEventRepository
{
    public async Task Add(Domain.Entities.Event eventEntity) => await dbContext.Events.AddAsync(eventEntity);

    public async Task<Domain.Entities.Event?> GetById(long id, long userId)
    {
        return await dbContext
            .Events
            .FirstOrDefaultAsync(eventEntity => eventEntity.Active && eventEntity.Id == id && eventEntity.UserId == userId);
    }

    public async Task<List<Domain.Entities.Event>> GetByDateRange(long userId, DateTime startDate, DateTime endDate, string? type)
    {
        var query = dbContext
            .Events
            .AsNoTracking()
            .Where(eventEntity => eventEntity.Active
                                  && eventEntity.UserId == userId
                                  && eventEntity.StartDateTime <= endDate
                                  && eventEntity.EndDateTime >= startDate);

        if (string.IsNullOrWhiteSpace(type) == false)
        {
            query = query.Where(eventEntity => eventEntity.Type.ToLower() == type.ToLower());
        }

        return await query
            .OrderBy(eventEntity => eventEntity.StartDateTime)
            .ToListAsync();
    }

    public void Update(Domain.Entities.Event eventEntity) => dbContext.Events.Update(eventEntity);

    public void Delete(Domain.Entities.Event eventEntity) => dbContext.Events.Remove(eventEntity);
}


