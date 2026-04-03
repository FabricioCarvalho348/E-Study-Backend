using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using EStudy.Application.UseCases.Token;
using EStudy.Communication.Requests.Tokens;
using EStudy.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.Token.RefreshToken;

public class UserRefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);

        var useCase = CreateUseCase(refreshToken);

        var result = await useCase.Execute(new RequestNewTokenJson
        {
            RefreshToken = refreshToken.Value
        });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_RefreshToken_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(new RequestNewTokenJson
        {
            RefreshToken = string.Empty
        });

        (await act.Should().ThrowAsync<RefreshTokenNotFoundException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.Auth.RefreshTokenNotFound);
    }

    [Fact]
    public async Task Error_RefreshToken_Expired()
    {
        var (user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedOn = DateTime.UtcNow.AddDays(-8);

        var useCase = CreateUseCase(refreshToken);

        var act = async () => await useCase.Execute(new RequestNewTokenJson
        {
            RefreshToken = refreshToken.Value
        });

        (await act.Should().ThrowAsync<RefreshTokenExpiredException>())
            .Where(e => e.GetErrors().Count == 1 && e.GetErrors().Single().Code == AppErrorCodes.Auth.RefreshTokenExpired);
    }

    private static UseRefreshTokenUseCase CreateUseCase(EStudy.Domain.Entities.RefreshToken? refreshToken = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Get(refreshToken).Build();

        return new UseRefreshTokenUseCase(unitOfWork, tokenRepository, refreshTokenGenerator, accessTokenGenerator);
    }
}