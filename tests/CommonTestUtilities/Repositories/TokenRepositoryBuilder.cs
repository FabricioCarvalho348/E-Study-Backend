using EStudy.Domain.Entities;
using EStudy.Domain.Repositories.Token;
using Moq;

namespace CommonTestUtilities.Repositories;

public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repository = new();

    public TokenRepositoryBuilder Get(RefreshToken? refreshToken)
    {
        if(refreshToken is not null)
            _repository.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public ITokenRepository Build() => _repository.Object;
}