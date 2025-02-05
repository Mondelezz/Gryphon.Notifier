namespace Application.Features.EventFeatures.Query;

public static partial class EventListGet
{
    public record ResponseDto(IReadOnlyList<EventDto> EventDtos, int TotalCount, int ActualEventsCount, int EndedEventsCount, decimal TotalPrice);

    public record EventDto(
        long EventId,
        string Name,
        string? Description,
        Importance Importance,
        decimal? Price,
        DateTime DateEvent,
        DateTime CreateDate,
        DateTime UpdateDate,
        GroupEventDto? GroupEventDto);

    public record GroupEventDto(
        long GroupEventId,
        string Name);

    /// <summary>
    /// Фильтры
    /// </summary>
    /// <param name="SearchTermFilter">Поисковый фильтр</param>
    /// <param name="GroupEventId">Идентификатор группы, к которой принадлежат события</param>
    /// <param name="IndicatedPriceFilter">Указана цена</param>
    /// <param name="IsCompleted">Завершено</param>
    public record RequestFilter(
        string? SearchTermFilter = default,
        long? GroupEventId = default,
        bool? IndicatedPriceFilter = default,
        bool? IsCompleted = default);

    public enum Sorting
    {
        Id = 0,
        DateEvent = 1,
        Importance = 2,
        Price = 3
    }
}
