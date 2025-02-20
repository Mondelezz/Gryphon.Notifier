using Riok.Mapperly.Abstractions;

namespace Application.Features.TopicFeatures.Query;

public static partial class GetTopic
{
    [Mapper]
    public static partial class Mapper
    {
        public static TopicDto Map(Topic source) =>
            new(source.Id,
                source.Name,
                source.TopicType,
                source.UserId,
                source.IsDeleted,
                source.CreateDate,
                source.UpdateDate,
                source.Events.Select(eventDb => new EventDto(
                    eventDb.Id,
                    eventDb.Name,
                    eventDb.Description,
                    eventDb.Importance,
                    eventDb.Price,
                    eventDb.IsIterative,
                    eventDb.TopicId,
                    eventDb.UserId,
                    eventDb.DateEvent,
                    eventDb.TimeEventStart,
                    eventDb.TimeEventEnded,
                    eventDb.CreateDate,
                    eventDb.UpdateDate)).ToList());
    }
}
