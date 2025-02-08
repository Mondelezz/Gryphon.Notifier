namespace Domain.Models;

/// <summary>
/// Отвечает за периодичность протекания событий
/// Пример: каждый вторник в 14:00; каждый понедельник, среду и пятницу в 15:00
/// </summary>
public class Periodicity
{
    /// <summary>
    /// Дни недели, в которые события повторяются
    /// </summary>
    public IList<DayOfWeek>? DayOfWeeks { get; set; } = [];

    /// <summary>
    /// Время, в которое начинается событие
    /// </summary>
    public TimeOnly? TimeEventStart { get; set; }
}
