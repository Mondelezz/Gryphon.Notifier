namespace Application.Features.EventFeatures.Query;

public static partial class GetEvent
{
    public record ResponseDto(EventDto EventDto);

    public record EventDto(
        long EventId,
        string Name,
        string? Description,
        Importance Importance,
        decimal? Price,
        bool IsIterative,
        long? TopicId,
        long UserId,
        bool IsDeleted,
        bool IsCompleted,
        TimeOnly? TimeEventStart,
        TimeOnly? TimeEventEnded,
        DateTime DateEvent,
        DateTime CreateDate,
        DateTime UpdateDate);
}
