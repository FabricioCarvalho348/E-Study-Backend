using EStudy.Communication.Requests.Tokens;
using EStudy.Communication.Responses.Tokens;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Token;
using EStudy.Domain.Security.Tokens;
using EStudy.Exception.ExceptionsBase;

namespace EStudy.Application.UseCases.Token;

public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public UseRefreshTokenUseCase(
        IUnitOfWork unitOfWork,
        ITokenRepository tokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _tokenRepository = tokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken);

        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(7);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
            RefreshToken = newRefreshToken.Value
        };
    }
}