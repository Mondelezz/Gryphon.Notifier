using Domain.Models.Event;
using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
{
    [Mapper]
    public static partial class Mapper
    {
        public static Event MapEventDtoToEvent(EventDto source, string userId) => new()
        {
            Name = source.Name,
            Description = source.Description,
            Importance = source.Importance,
            DateEvent = source.DateEvent,
            Price = source.Price,
            IsIterative = source.IsIterative,
            UserId = userId,
        };
    }
}
