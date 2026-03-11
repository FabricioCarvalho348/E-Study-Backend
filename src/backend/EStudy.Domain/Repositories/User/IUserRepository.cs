namespace EStudy.Domain.Repositories.User;

public interface IUserRepository
{
    public Task Add(Entities.User user);
    Task DeleteAccount(Guid userIdentifier);
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);

    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
    public Task<Entities.User> GetById(long id);
    public void Update(Entities.User user);
}