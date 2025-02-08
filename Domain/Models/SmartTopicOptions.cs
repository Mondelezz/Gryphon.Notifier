using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

/// <summary>
/// Опции интеллекутального топика.
/// Все события, которые создаются в умном топике, будут унифицированы под заданные настройки
/// </summary>
public class SmartTopicOptions : EntityBase
{
    /// <summary>
    /// Важность события
    /// </summary>
    public Importance? Importance { get; set; }
}
