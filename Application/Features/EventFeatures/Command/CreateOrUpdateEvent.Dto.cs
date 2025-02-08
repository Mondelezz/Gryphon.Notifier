namespace Application.Features.EventFeatures.Command;

public static partial class CreateOrUpdateEvent
{
    public record RequestDto(EventDto EventDto);

    public record EventDto(
        string Name,
        string? Description,
        Importance Importance,
        DateTime DateEvent,
        TimeOnly? TimeEventStart,
        TimeOnly? TimeEventEnded,
        decimal? Price,
        bool IsIterative);
}
