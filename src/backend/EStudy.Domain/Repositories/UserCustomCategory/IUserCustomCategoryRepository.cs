namespace EStudy.Domain.Repositories.UserCustomCategory;

public interface IUserCustomCategoryRepository
{
    Task Add(Entities.UserCustomCategory category);
    Task<List<Entities.UserCustomCategory>> GetAllByUserId(long userId);
    Task<Entities.UserCustomCategory?> GetById(long id, long userId);
    void Delete(Entities.UserCustomCategory category);
    void Update(Entities.UserCustomCategory category);
}

