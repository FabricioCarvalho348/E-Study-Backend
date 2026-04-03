using EStudy.Domain.Entities;
using EStudy.Domain.Repositories.UserTask;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess.Repositories;

public class UserTaskRepository(EStudyDbContext dbContext) : IUserTaskRepository
{
    public async Task Add(UserTask userTask) => await dbContext.UserTasks.AddAsync(userTask);

    public async Task<List<UserTask>> GetAllByUserId(long userId)
    {
        return await dbContext
            .UserTasks
            .AsNoTracking()
            .Where(userTask => userTask.Active && userTask.UserId == userId)
            .OrderByDescending(userTask => userTask.CreatedOn)
            .ToListAsync();
    }

    public async Task<UserTask?> GetById(long id, long userId)
    {
        return await dbContext
            .UserTasks
            .FirstOrDefaultAsync(userTask => userTask.Active && userTask.Id == id && userTask.UserId == userId);
    }

    public void Delete(UserTask userTask)
    {
        dbContext.UserTasks.Remove(userTask);
    }

    public void Update(UserTask userTask) => dbContext.UserTasks.Update(userTask);
}
