using Domain.Models;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<bool> CreateUserTokenAsync(UserToken userToken, CancellationToken cancellationToken)
    {
        try
        {
            await AddAsync(userToken, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex) when (ex is DbUpdateException or OperationCanceledException)
        {
            return false;
        }
    }

    public async Task<UserToken?> GetByUserAndLoginIdAsync(long userId, long userLoginId, CancellationToken cancellationToken) => await
        queryDbContext.UserTokens
        .Where(ut => ut.UserId == userId && ut.UserLoginId == userLoginId)
        .FirstOrDefaultAsync(cancellationToken);
}
