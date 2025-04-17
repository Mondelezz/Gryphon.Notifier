using Domain.Models;

namespace Domain.Interfaces;

public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    public Task<bool> CreateUserTokenAsync(UserToken userToken, CancellationToken cancellationToken);

    public Task<UserToken?> GetByUserAndLoginIdAsync(long userId, long userLoginId, CancellationToken cancellationToken);
}
