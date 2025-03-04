using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

/// <summary>
/// Топик - логическое разделение событий на группы
/// </summary>
public class Topic : EntityBase, IEntityState
{
    /// <summary>
    /// Наименование топика
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Тип топика, от которого зависит первоначальная настройка событий входящих в топик
    /// </summary>
    public required TopicType TopicType { get; set; }

    /// <summary>
    /// События
    /// </summary>
    public IList<Event> Events { get; set; } = [];

    /// <summary>
    /// Идентификатор пользователя, который создал топик
    /// </summary>
    public required long UserId { get; set; }

    /// <summary>
    /// Сущность удалена
    /// </summary>
    public bool IsDeleted { get; set; }
}
