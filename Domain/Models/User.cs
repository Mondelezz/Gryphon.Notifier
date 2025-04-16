using Domain.Common;

namespace Domain.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User : EntityBase, IEntityDate
{
    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Никнейм
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public required string Email { get; set; }

    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// Логин-провайдер пользователя
    /// </summary>
    public IList<UserLogin> UserLogins { get; set; } = [];

    /// <summary>
    /// Список токенов пользователя для обеспечения нескольких сессий
    /// </summary>
    public IList<UserToken> UserTokens { get; set; } = [];
}
