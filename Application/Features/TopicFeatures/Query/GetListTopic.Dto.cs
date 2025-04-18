namespace Application.Features.TopicFeatures.Query;

public static partial class GetListTopic
{
    public record ResponseDto(IReadOnlyList<TopicDto> TopicDtos, int TotalCount);

    public record TopicDto(
        long TopicId,
        long UserId,
        string Name,
        DateTime CreateDate,
        DateTime UpdateDate);
}
