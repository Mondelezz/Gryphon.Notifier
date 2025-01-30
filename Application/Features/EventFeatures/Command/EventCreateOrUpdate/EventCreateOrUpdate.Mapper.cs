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
            Price = source.Price,
            IsIterative = source.IsIterative,

            DateEvent = source.DateEvent,
            TimeEventStart = source.TimeEventStart,
            TimeEventEnded = source.TimeEventEnded,

            UserId = userId,
            GroupEventId = groupEventId
        };

        public static void Map(Event destination, EventDto source, string userId, long? groupEventId)
        {
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Importance = source.Importance;
            destination.Price = source.Price;
            destination.IsIterative = source.IsIterative;

            destination.DateEvent = source.DateEvent;
            destination.TimeEventStart = source.TimeEventStart;
            destination.TimeEventEnded = source.TimeEventEnded;

            destination.UserId = userId;
            destination.GroupEventId = groupEventId;
        }
    }
}
