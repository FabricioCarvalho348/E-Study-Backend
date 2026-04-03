using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using EStudy.Application.UseCases.User.ChangePassword;
using EStudy.Communication.Requests.ChangePassword;
using EStudy.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var (user, password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var (user, password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);
    }

    private static ChangePasswordUseCase CreateUseCase(EStudy.Domain.Entities.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncrypter, userUpdateRepository, unitOfWork);
    }
}