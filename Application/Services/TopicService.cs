using Application.Features.TopicFeatures.Command;
using Application.Interfaces;

using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Базовый класс сервиса, содержащий приватные методы, используемые в частичных классах сервиса
/// </summary>
/// <param name="topicRepository">Репозиторий топиков</param>
public partial class TopicService(ITopicRepository topicRepository) : ITopicService
{
    private async Task<long> CreateTopicAsync(long userId, CreateOrUpdateTopic.RequestDto request, CancellationToken cancellationToken)
    {
        Topic topic = CreateOrUpdateTopic.Mapper.Map(request.TopicDto, userId);

        await topicRepository.AddTopicAsync(topic, cancellationToken);

        return topic.Id;
    }

    private async Task<long> UpdateTopicAsync(long userId, long topicId, CreateOrUpdateTopic.RequestDto request, CancellationToken cancellationToken)
    {
        Topic topic = await GetTopicByIdAsync(userId, topicId, cancellationToken)
            ?? throw new EntityNotFoundException(topicId, userId, nameof(topicId), nameof(userId));

        // Обновление полей сущности
        CreateOrUpdateTopic.Mapper.Map(topic, request.TopicDto);

        await topicRepository.UpdateAsync(topic, cancellationToken);

        return topic.Id;
    }
}
