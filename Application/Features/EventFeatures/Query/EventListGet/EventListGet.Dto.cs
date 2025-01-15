using Domain.Enums;

namespace Application.Features.EventFeatures.Query.EventListGet;

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

    public record RequestFilter(
        string? SearchTermFilter = default,
        long? GroupEventId = default,
        bool? IndicatedPriceFilter = default);

    public enum Sorting
    {
        Id,
        DateEvent,
        Importance,
        Price
    }
}
