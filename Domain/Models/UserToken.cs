using Domain.Common;

namespace Domain.Models;

public class UserToken : EntityBase
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Время истечения срока действия AccessToken
    /// </summary>
    public DateTime AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// Токен, через который получаем новый Access token
    /// </summary>
    public required string RefreshToken { get; set; }

    public required long UserId { get; set; }

    public required UserLogin UserLogin { get; set; }
    public required long UserLoginId { get; set; }
}
