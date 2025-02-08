namespace Application.Features.EventFeatures.Query;

public static partial class GetListTopic
{
    public record ResponseDto(IReadOnlyList<TopicDto> TopicDtos, int TotalCount);

    public record TopicDto(
        long TopicId,
        string UserId,
        string Name,
        DateTime CreateDate,
        DateTime UpdateDate);
}
