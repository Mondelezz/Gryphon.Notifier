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
            GroupEventType = source.GroupEventType,
            UserId = userId,
        };

        public static void Map(GroupEvent destination, GroupEventDto source)
        {
            destination.Name = source.Name;
            destination.GroupEventType = source.GroupEventType;
        }
    }
}
