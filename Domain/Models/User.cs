using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User : EntityBase
{
    /// <summary>
    /// Email пользователя (для консистентности данных и единого аккуанта среди различных провайдеров аутентификации)
    /// Email необходимо подтвердить
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Провайдеры, через которые авторизовался пользователь
    /// </summary>
    public IList<ExternalIdentity> ExternalIdentities { get; set; } = [];
}
