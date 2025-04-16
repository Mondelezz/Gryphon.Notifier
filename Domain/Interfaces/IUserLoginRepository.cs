using Domain.Models;

namespace Domain.Interfaces;

public interface IUserLoginRepository
{
    Task<UserLogin?> FindByProviderNameAsync(string providerName, string email, CancellationToken cancellationToken);

    Task<bool> CreateUserLoginAsync(UserLogin userLogin, CancellationToken cancellationToken);
}
