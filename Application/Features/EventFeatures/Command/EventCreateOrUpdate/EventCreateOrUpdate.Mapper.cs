using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Command.EventCreateOrUpdate;

public static partial class EventCreateOrUpdate
{
    [Mapper]
    public static partial class Mapper
    {
        public static Event Map(EventDto source, string userId, long? groupEventId) => new()
        {
            Name = source.Name,
            Description = source.Description,
            Importance = source.Importance,
            DateEvent = source.DateEvent,
            Price = source.Price,
            IsIterative = source.IsIterative,
            UserId = userId,
            GroupEventId = groupEventId
        };
    }
}
