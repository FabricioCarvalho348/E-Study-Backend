using EStudy.Domain.Entities;

namespace EStudy.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}