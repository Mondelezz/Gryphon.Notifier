using Domain.Common;

namespace Domain.Models.Event;

/// <summary>
/// Группа событий
/// </summary>
public class GroupEvent : EntityBase, IEntityState
{
    /// <summary>
    /// Наименование группы
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// События
    /// </summary>
    public IList<Event> Events { get; set; } = [];

    /// <summary>
    /// Идентификатор пользователя который создал  группу для событий
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Сущность удалена
    /// </summary>
    public bool IsDeleted { get; set; }
}
