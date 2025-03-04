using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

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
    /// Время начала события (для событий, которые начинаются с определённого момента времени) Пример: Тренировка
    /// </summary>
    public TimeOnly? TimeEventStart { get; set; }

    /// <summary>
    /// Время окончания события
    /// (для событий, которые заканчиваются в определённый момент времени) Пример: Тренировка
    /// </summary>
    public TimeOnly? TimeEventEnded { get; set; }

    /// <summary>
    /// Цена, которая принадлежит событию ( услуга / тариф )
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Повторяющееся событие в определённую дату
    /// (система автоматически пересоздаёт завершённые события в зависимости от настройки периодичности)
    /// </summary>
    public bool IsIterative { get; set; }

    /// <summary>
    /// Топик
    /// </summary>
    public Topic? Topic { get; set; }

    public long? TopicId { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required long UserId { get; set; }

    /// <summary>
    /// Событие удалено
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Событие завершено
    /// </summary>
    public bool IsCompleted { get; set; }
}
