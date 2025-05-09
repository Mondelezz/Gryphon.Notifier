using Application.Features.TopicFeatures.Query;

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
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<GetTopic.ResponseDto> GetTopicByIdAsync(long userId, long topicId, CancellationToken cancellationToken)
    {
        Topic topic = await topicRepository.GetTopicByIdAsync(topicId, userId, cancellationToken)
        ?? throw new EntityNotFoundException(topicId, userId, nameof(topicId), nameof(userId));

        return new GetTopic.ResponseDto(GetTopic.Mapper.Map(topic));
    }
}
