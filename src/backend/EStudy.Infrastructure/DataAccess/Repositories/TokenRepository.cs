using EStudy.Domain.Entities;
using EStudy.Domain.Repositories.Token;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess.Repositories
{
    public class TokenRepository(EStudyDbContext dbContext) : ITokenRepository
    {
        public async Task<RefreshToken?> Get(string refreshToken)
        {
            return await dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.User)
                .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

            dbContext.RefreshTokens.RemoveRange(tokens);

            await dbContext.RefreshTokens.AddAsync(refreshToken);
        }
    }
}