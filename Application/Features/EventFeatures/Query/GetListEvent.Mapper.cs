using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Query;

public static partial class GetListEvent
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
                eventdB.Topic != null
                ? Map(eventdB.Topic)
                : null))
            .ToList();

        public static TopicDto Map(Topic source) => new(source.Id, source.Name);
    }
}
