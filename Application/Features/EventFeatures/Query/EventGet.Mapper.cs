using Domain.Models.Event;
using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Query;

public static partial class EventGet
{
    [Mapper]
    public static partial class Mapper
    {
        public static EventDto Map(Event source) => new(
            source.Id,
            source.Name,
            source.Description,
            source.Importance,
            source.Price,
            source.IsIterative,
            source.GroupEventId,
            source.UserId,
            source.DateEvent,
            source.CreateDate,
            source.UpdateDate);
    }
}
