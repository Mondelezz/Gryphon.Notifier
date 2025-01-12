using Domain.Models.Event;
using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Command.GroupEventCreateOrUpdate;

public static partial class GroupEventCreateOrUpdate
{
    [Mapper]
    public static partial class Mapper
    {
        public static GroupEvent Map(GroupEventDto source, string userId) => new()
        {
            Name = source.Name,
            UserId = userId,
        };
    }
}
