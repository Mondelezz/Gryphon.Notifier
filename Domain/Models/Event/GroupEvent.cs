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

    public bool IsDeleted { get; set; }
}
