using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Extensions;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using EStudy.Application.UseCases.User.Register;
using EStudy.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
        [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.General.Validation);
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (email.NotEmpty())
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), unitOfWork, passwordEncrypter, accessTokenGenerator, mapper, tokenRepository, refreshTokenGenerator);
    }
}