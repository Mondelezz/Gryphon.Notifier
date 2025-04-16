using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken);
}
