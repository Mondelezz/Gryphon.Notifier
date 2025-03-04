namespace Application.Features.TopicFeatures.Query;

public static partial class GetTopic
{
    public record ResponseDto(TopicDto TopicDto);

    public record TopicDto(
        long TopicId,
        string Name,
        TopicType TopicType,
        long UserId,
        bool IsDeleted,
        DateTime CreatedDate,
        DateTime UpdateDate,
        IReadOnlyList<EventDto> EventDtos);

    public record EventDto(
        long EventId,
        string Name,
        string? Description,
        Importance Importance,
        decimal? Price,
        bool IsIterative,
        long? TopicId,
        long UserId,
        DateTime DateEvent,
        TimeOnly? TimeEventStart,
        TimeOnly? TimeEventEnded,
        DateTime CreateDate,
        DateTime UpdateDate);
}
