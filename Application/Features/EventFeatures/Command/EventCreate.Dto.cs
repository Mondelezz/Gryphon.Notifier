using Domain.Enums;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
{
    public record RequestDto(EventDto EventDto);

    public record EventDto(
        string Name,
        string? Description,
        Importance Importance,
        DateTime DateEvent,
        decimal? Price,
        bool? IsIterative,
        GroupEventDto? GroupEventDto);

    public record GroupEventDto(
        long? GroupEventId);
}
