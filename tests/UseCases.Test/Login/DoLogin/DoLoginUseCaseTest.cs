using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using EStudy.Application.UseCases.Login.DoLogin;
using EStudy.Communication.Requests.DoLogin;
using EStudy.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_When_UserIdentifier_Is_Empty()
    {
        var (user, password) = UserBuilder.Build();
        user.UserIdentifier = Guid.Empty;

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        user.UserIdentifier.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.Auth.InvalidCredentials);
    }

    private static DoLoginUseCase CreateUseCase(EStudy.Domain.Entities.User? user = null)
    {
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var userUpdateOnlyRepositoryBuilder = new UserUpdateOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (user is not null)
        {
            userReadOnlyRepositoryBuilder.GetByEmail(user);
            userUpdateOnlyRepositoryBuilder.GetById(user);
        }

        return new DoLoginUseCase(
            userReadOnlyRepositoryBuilder.Build(),
            userUpdateOnlyRepositoryBuilder.Build(),
            accessTokenGenerator,
            passwordEncrypter,
            refreshTokenGenerator,
            tokenRepository,
            unitOfWork);
    }
}