using Bogus;
using EStudy.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserCustomCategoryBuilder
{
    public static UserCustomCategory Build(
        long id = 1,
        string name = "Default Category",
        long userId = 1,
        User? user = null)
    {
        return new Faker<UserCustomCategory>()
            .RuleFor(category => category.Id, _ => id)
            .RuleFor(category => category.Name, _ => name)
            .RuleFor(category => category.UserId, _ => userId)
            .RuleFor(category => category.User, _ => user ?? new User { Id = userId })
            .RuleFor(category => category.UserTasks, _ => new List<UserTask>());
    }
}
