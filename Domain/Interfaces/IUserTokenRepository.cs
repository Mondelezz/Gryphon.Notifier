using Domain.Models;

namespace Domain.Interfaces;

public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    /// <summary>
    /// Создаёт сущность UserToken
    /// </summary>
    /// <param name="userToken">Токен доступа и токен обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность создана</returns>
    public Task<bool> CreateUserTokenAsync(UserToken userToken, CancellationToken cancellationToken);

    /// <summary>
    /// Получает сущность UserToken по userId и userLoginId
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userLoginId">Идентификатор логин-провайдера</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность UserToken</returns>
    public Task<UserToken?> GetByUserAndLoginIdAsync(long userId, long userLoginId, CancellationToken cancellationToken);
}
