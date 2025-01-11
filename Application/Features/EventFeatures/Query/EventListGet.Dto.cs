using Domain.Enums;

namespace Application.Features.EventFeatures.Query;

public static partial class EventListGet
{
    public record ResponseDto(IReadOnlyList<EventDto> EventDtos, int TotalCount);

    public record EventDto(
        long EventId,
        string Name,
        string? Description,
        Importance Importance,
        decimal? Price,
        DateTime DateEvent,
        GroupEventDto? GroupEventDto);

    public record GroupEventDto(
        long EventGroupId,
        string Name);

    public record RequestFilter(
        string? SearchTermFilter = default,
        bool? IndicatedPriceFilter = default);

    public enum Sorting
    {
        DateEvent,
        Importance,
        Price
    }
}
