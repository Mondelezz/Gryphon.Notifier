using Riok.Mapperly.Abstractions;

namespace Application.Features.TopicFeatures.Command;

public static partial class CreateOrUpdateTopic
{
    [Mapper]
    public static partial class Mapper
    {
        public static Topic Map(TopicDto source, string userId) => new()
        {
            Name = source.Name,
            TopicType = source.TopicType,
            UserId = userId,
        };

        public static void Map(Topic destination, TopicDto source)
        {
            destination.Name = source.Name;
            destination.TopicType = source.TopicType;
        }
    }
}
