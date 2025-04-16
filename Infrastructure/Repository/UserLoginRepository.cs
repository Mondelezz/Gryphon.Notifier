using Domain.Interfaces;
using Domain.Models;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal class UserLoginRepository : BaseRepository<UserLogin>, IUserLoginRepository
{
    public UserLoginRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<UserLogin?> FindByProviderNameAsync(string providerName, string email, CancellationToken cancellationToken) => await
        queryDbContext.UserLogins.Where(ul => ul.LoginProvider == providerName && ul.Email == email).FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> CreateUserLoginAsync(UserLogin userLogin, CancellationToken cancellationToken)
    {
        try
        {
            await AddAsync(userLogin, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex) when (ex is DbUpdateException or OperationCanceledException)
        {
            return false;
        }
    }
}
