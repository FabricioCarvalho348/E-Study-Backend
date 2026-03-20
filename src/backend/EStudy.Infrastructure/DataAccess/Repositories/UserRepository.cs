using EStudy.Domain.Entities;
using EStudy.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess.Repositories;

public class UserRepository(EStudyDbContext dbContext) : IUserRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);
    
    public async Task<bool> ExistActiveUserWithEmail(string email) => await dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
    
    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<User> GetById(long id)
    {
        return await dbContext
            .Users
            .FirstAsync(user => user.Id == id);
    }
    
    public void Update(User user) => dbContext.Users.Update(user);
    
    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.UserIdentifier.Equals(userIdentifier));
        
        if (user is null)
            return;
        
        dbContext.Users.Remove(user);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }
}
