using Domain.Common;
using Domain.Enums;

namespace Domain.Models.Event;

public class Event : EntityBase, IEntityState
{
    /// <summary>
    /// Наименование события
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Описание события
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Важность события
    /// </summary>
    public required Importance Importance { get; set; }

    /// <summary>
    /// Дата события
    /// </summary>
    public required DateTime DateEvent { get; set; }

    /// <summary>
    /// Время начала события
    /// </summary>
    public DateTime? TimeEventStart { get; set; }

    /// <summary>
    /// Время окончания события
    /// </summary>
    public DateTime? TimeEventEnded { get; set; }

    /// <summary>
    /// Стоимость события ( услуга / тариф )
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Повторяющееся событие в определённую дату
    /// </summary>
    public bool IsIterative { get; set; }

    /// <summary>
    /// Группа
    /// </summary>
    public GroupEvent? GroupEvent { get; set; }

    public long? GroupEventId { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required string UserId { get; set; }

    public bool IsDeleted { get; set; }
}
