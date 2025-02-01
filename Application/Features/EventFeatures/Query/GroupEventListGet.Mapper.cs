using Riok.Mapperly.Abstractions;

namespace Application.Features.EventFeatures.Query;

public static partial class GroupEventListGet
{
    [Mapper]
    public static partial class Mapper
    {
        public static IReadOnlyList<GroupEventDto> Map(IReadOnlyList<GroupEvent> source) =>
            source.Select(groupEventDb => new GroupEventDto(
                groupEventDb.Id,
                groupEventDb.UserId,
                groupEventDb.Name,
                groupEventDb.CreateDate,
                groupEventDb.UpdateDate)).ToList();

    }
}
