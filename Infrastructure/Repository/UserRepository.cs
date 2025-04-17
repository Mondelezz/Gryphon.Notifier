using Domain.Interfaces;
using Domain.Models;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await AddAsync(user, cancellationToken);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex) when (ex is DbUpdateException or OperationCanceledException)
        {
            return false;
        }
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken) => await
        queryDbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
}
