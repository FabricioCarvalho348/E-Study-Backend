using CommonTestUtilities.Entities;
using CommonTestUtilities.Extensions;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using EStudy.Application.UseCases.User.Update;
using EStudy.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(EStudy.Domain.Entities.User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        if (email.NotEmpty())
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}