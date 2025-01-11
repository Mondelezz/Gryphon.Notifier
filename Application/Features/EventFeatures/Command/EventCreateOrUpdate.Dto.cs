using Domain.Enums;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreateOrUpdate
{
    public record RequestDto(EventCreateOrUpdateDto EventCreateOrUpdateDto);

    public record EventCreateOrUpdateDto(
        string Name,
        string? Description,
        Importance Importance,
        DateTime DateEvent,
        decimal? Price,
        bool IsIterative);
}
