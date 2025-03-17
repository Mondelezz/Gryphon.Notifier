using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Внешний провайдер
/// </summary>
/// Получаем ExternalUserId, который нам передаёт провайдер аутентификации
/// Проверяем, существует ли пользователь с Email-ом, который нам передал провайдер, либо пользователь указал вручную.
/// Если email существует - создаётся данная сущность, иначе созадётся сущность User (новый пользователь)
public class ExternalIdentity : EntityBase
{
    public required User User { get; set; }
    public required long UserId { get; set; }

    /// <summary>
    /// Провайдер аутентификации "Google", "VK", "Telegram" и т.д.
    /// </summary>
    public required string Provider { get; set; }

    /// <summary>
    /// Идентификатор пользователя из провайдера
    /// </summary>
    public required string ExternalUserId { get; set; }
}
