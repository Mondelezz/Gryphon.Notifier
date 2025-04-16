using Domain.Common;

namespace Domain.Models;

public class UserLogin : EntityBase
{
    /// <summary>
    /// Логин-провайдер
    /// </summary>
    public required string LoginProvider { get; set; }

    public required string Email { get; set; }

    public required long UserId { get; set; }
}
