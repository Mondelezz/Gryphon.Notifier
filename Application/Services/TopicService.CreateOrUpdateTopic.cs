using Application.Features.TopicFeatures.Command;

namespace Application.Services;

public partial class TopicService
{
    /// <summary>
    /// Создаёт или обновляет топик
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="topicDto">Данные топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданного/обновлённого топика</returns>
    public async Task<long> CreateOrUpdateTopicAsync(long userId, long? topicId, CreateOrUpdateTopic.RequestDto topicDto, CancellationToken cancellationToken)
    {
        if (topicId is null)
        {
            return await CreateTopicAsync(userId, topicDto, cancellationToken);
        }

        return await UpdateTopicAsync(userId, topicId.Value, topicDto, cancellationToken);
    }
}
