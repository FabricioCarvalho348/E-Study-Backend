using EStudy.Communication.Requests.DoLogin;
using EStudy.Communication.Responses.Tokens;
using EStudy.Communication.Responses.Users;
using EStudy.Domain.Extensions;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Token;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IAccessTokenGenerator accessTokenGenerator,
    IPasswordEncrypter passwordEncrypter,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenRepository tokenRepository,
    IUnitOfWork unitOfWork)
    : IDoLoginUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await repository.GetByEmail(request.Email);

        if (user is null || passwordEncrypter.IsValid(request.Password, user.Password).IsFalse())
            throw new InvalidLoginException();
        
        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new Domain.Entities.RefreshToken
        {
            Value = refreshTokenGenerator.Generate(),
            UserId = user.Id
        };

        await tokenRepository.SaveNewRefreshToken(refreshToken);

        await unitOfWork.Commit();

        return refreshToken.Value;
    }
}