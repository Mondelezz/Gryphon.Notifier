namespace Application.Features.EventFeatures.Command.GroupEventCreateOrUpdate;

public static partial class GroupEventCreateOrUpdate
{
    public record RequestDto(GroupEventDto GroupEventDto);

    public record GroupEventDto(
        string Name,
        GroupEventType GroupEventType);
}
