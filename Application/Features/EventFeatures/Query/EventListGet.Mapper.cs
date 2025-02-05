using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Query;

public static partial class EventListGet
{
    [Mapper]
    public static partial class Mapper
    {
        public static IReadOnlyList<EventDto> Map(IReadOnlyList<Event> source) =>
            source.Select(eventdB => new EventDto(
                eventdB.Id,
                eventdB.Name,
                eventdB.Description,
                eventdB.Importance,
                eventdB.Price,
                eventdB.IsCompleted,
                eventdB.DateEvent,
                eventdB.CreateDate,
                eventdB.UpdateDate,
                eventdB.GroupEvent != null
                ? Map(eventdB.GroupEvent)
                : null))
            .ToList();

        public static GroupEventDto Map(GroupEvent source) => new(source.Id, source.Name);
    }
}
