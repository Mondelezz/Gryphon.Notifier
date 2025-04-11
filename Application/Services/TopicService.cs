using Microsoft.EntityFrameworkCore;

using Application.Features.TopicFeatures.Command;
using Application.Features.TopicFeatures.Query;
using Application.Interfaces;

using Domain.Interfaces;

namespace Application.Services;

public class TopicService(ITopicRepository topicRepository) : ITopicService
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

    /// <summary>
    /// Получает список топиков
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    public async Task<GetListTopic.ResponseDto> GetListTopicAsync(long userId, CancellationToken cancellationToken)
    {
        IReadOnlyList<Topic> topics = await topicRepository
            .GetAll()
            .Where(t => t.UserId == userId && !t.IsDeleted)
            .OrderByDescending(t => t.UpdateDate)
            .ToListAsync(cancellationToken);

        int totalCount = topics.Count;

        if (totalCount == 0)
        {
            return new GetListTopic.ResponseDto([], totalCount);
        }

        return new GetListTopic.ResponseDto(GetListTopic.Mapper.Map(topics), totalCount);
    }

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

    private async Task<long> CreateTopicAsync(long userId, CreateOrUpdateTopic.RequestDto request, CancellationToken cancellationToken)
    {
        Topic topic = CreateOrUpdateTopic.Mapper.Map(request.TopicDto, userId);

        await topicRepository.AddTopicAsync(topic, cancellationToken);

        return topic.Id;
    }

    private async Task<long> UpdateTopicAsync(long userId, long topicId, CreateOrUpdateTopic.RequestDto request, CancellationToken cancellationToken)
    {
        Topic topic = await topicRepository.GetTopicByIdAsync(topicId, userId, cancellationToken)
            ?? throw new EntityNotFoundException(topicId, userId, nameof(topicId), nameof(userId));

        // Обновление полей сущности
        CreateOrUpdateTopic.Mapper.Map(topic, request.TopicDto);

        await topicRepository.UpdateAsync(topic, cancellationToken);

        return topic.Id;
    }
}
