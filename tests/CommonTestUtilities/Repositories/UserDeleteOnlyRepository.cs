using EStudy.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserDeleteOnlyRepository
{
    public static IUserDeleteOnlyRepository Build()
    {
        var mock = new Mock<IUserDeleteOnlyRepository>();

        return mock.Object;
    }
}