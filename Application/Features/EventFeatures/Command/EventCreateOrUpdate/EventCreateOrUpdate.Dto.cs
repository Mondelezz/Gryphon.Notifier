namespace Application.Features.EventFeatures.Command.EventCreateOrUpdate;

public static partial class EventCreateOrUpdate
{
    public record RequestDto(EventDto EventDto);

    public record EventDto(
        string Name,
        string? Description,
        Importance Importance,
        DateTime DateEvent,
        DateTime? TimeEventStart,
        DateTime? TimeEventEnded,
        decimal? Price,
        bool IsIterative);
}
