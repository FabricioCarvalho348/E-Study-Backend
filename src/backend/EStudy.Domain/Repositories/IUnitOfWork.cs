namespace EStudy.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}