using Domain.Common;

using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User : IdentityUser<long>, IEntityDate
{
    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия
    /// </summary>
    public required string LastName { get; set; } = string.Empty;
}
