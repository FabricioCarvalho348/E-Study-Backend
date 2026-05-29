using EStudy.Domain.Entities;
using EStudy.Domain.Repositories.UserCustomCategory;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess.Repositories;

public class UserCustomCategoryRepository(EStudyDbContext dbContext) : IUserCustomCategoryRepository
{
    public async Task Add(UserCustomCategory category) => await dbContext.UserCustomCategories.AddAsync(category);

    public async Task<List<UserCustomCategory>> GetAllByUserId(long userId)
    {
        return await dbContext
            .UserCustomCategories
            .AsNoTracking()
            .Where(category => category.Active && category.UserId == userId)
            .OrderByDescending(category => category.CreatedOn)
            .ToListAsync();
    }

    public async Task<UserCustomCategory?> GetById(long id, long userId)
    {
        return await dbContext
            .UserCustomCategories
            .FirstOrDefaultAsync(category => category.Active && category.Id == id && category.UserId == userId);
    }

    public void Delete(UserCustomCategory category)
    {
        dbContext.UserCustomCategories.Remove(category);
    }

    public void Update(UserCustomCategory category) => dbContext.UserCustomCategories.Update(category);
}

