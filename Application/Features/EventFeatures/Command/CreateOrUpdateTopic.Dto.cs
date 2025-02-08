namespace Application.Features.EventFeatures.Command;

public static partial class CreateOrUpdateTopic
{
    public record RequestDto(TopicDto TopicDto);

    /// <summary>
    /// Топик
    /// </summary>
    /// <param name="Name">Название топика</param>
    /// <param name="TopicType">Тип топика</param>
    public record TopicDto(
        string Name,
        TopicType TopicType);
}
