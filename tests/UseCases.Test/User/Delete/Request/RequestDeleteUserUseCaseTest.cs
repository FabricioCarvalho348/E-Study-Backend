using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using EStudy.Application.UseCases.User.Delete.Request;
using FluentAssertions;

namespace UseCases.Test.User.Delete.Request;

public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();

        user.Active.Should().BeFalse();
    }

    private static RequestDeleteUserUseCase CreateUseCase(EStudy.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

        return new RequestDeleteUserUseCase(repository, loggedUser, unitOfWork);
    }
}