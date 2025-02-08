namespace Application.Features.EventFeatures.Query;

public static partial class GetListEvent
{
    public record ResponseDto(
        IReadOnlyList<EventDto> EventDtos,
        int TotalCount,
        int ActualEventsCount,
        int EndedEventsCount,
        decimal TotalPrice);

    public record EventDto(
        long EventId,
        string Name,
        string? Description,
        Importance Importance,
        decimal? Price,
        bool IsCompleted,
        DateTime DateEvent,
        DateTime CreateDate,
        DateTime UpdateDate,
        TopicDto? TopicDto);

    public record TopicDto(
        long TopicId,
        string Name);

    /// <summary>
    /// Фильтры
    /// </summary>
    /// <param name="SearchTermFilter">Поисковый фильтр</param>
    /// <param name="TopicId">Идентификатор топика, к которому принадлежат события</param>
    /// <param name="IndicatedPriceFilter">Указана цена</param>
    /// <param name="IsCompleted">Завершено</param>
    public record RequestFilter(
        string? SearchTermFilter = default,
        long? TopicId = default,
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
