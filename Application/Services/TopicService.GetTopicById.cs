namespace Application.Services;

public partial class TopicService
{
    /// <summary>
    /// Получает топик по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Топик и связанные с ним события</returns>
    /// <exception cref="EntityNotFoundException">NRE</exception>
    /// <exception cref="AccessException">Ошибка доступа</exception>
    public async Task<Topic> GetTopicByIdAsync(long userId, long topicId, CancellationToken cancellationToken)
    {
        Topic topic = await topicRepository.GetTopicByIdAsync(topicId, cancellationToken)
        ?? throw new EntityNotFoundException(topicId, nameof(topicId));

        if (topic.UserId != userId)
        {
            throw new AccessException($"Ошибка доступа к топику: {topicId} для пользователя: {userId}");
        }

        return topic;
    }
}
