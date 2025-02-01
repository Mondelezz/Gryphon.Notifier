namespace Application.Features.EventFeatures.Command;

public static partial class GroupEventCreateOrUpdate
{
    public record RequestDto(GroupEventDto GroupEventDto);

    /// <summary>
    /// Модель создаваемой группы для событий
    /// </summary>
    /// <param name="Name">Название группы</param>
    /// <param name="GroupEventType">Тип группы</param>
    public record GroupEventDto(
        string Name,
        GroupEventType GroupEventType);
}
