using EStudy.Domain.Repositories;
using EStudy.Infrastructure.DataAccess.Repositories;

namespace EStudy.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly EStudyDbContext _dbContext;
    
    public UnitOfWork(EStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}