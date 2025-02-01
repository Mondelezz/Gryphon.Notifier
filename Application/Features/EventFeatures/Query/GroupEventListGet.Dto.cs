namespace Application.Features.EventFeatures.Query;

public static partial class GroupEventListGet
{
    public record ResponseDto(IReadOnlyList<GroupEventDto> GroupEventDto, int TotalCount);

    public record GroupEventDto(
        long GroupEventId,
        string UserId,
        string Name,
        DateTime CreateDate,
        DateTime UpdateDate);
}
