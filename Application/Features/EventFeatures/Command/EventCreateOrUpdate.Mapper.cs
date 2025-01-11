using Domain.Models.Event;
using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreateOrUpdate
{
    [Mapper]
    public static partial class Mapper
    {
        public static Event Map(EventCreateOrUpdateDto source, string userId) => new()
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
