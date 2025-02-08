using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Query;

public static partial class GetListTopic
{
    [Mapper]
    public static partial class Mapper
    {
        public static IReadOnlyList<TopicDto> Map(IReadOnlyList<Topic> source) =>
            source.Select(topicDb => new TopicDto(
                topicDb.Id,
                topicDb.UserId,
                topicDb.Name,
                topicDb.CreateDate,
                topicDb.UpdateDate)).ToList();
    }
}
