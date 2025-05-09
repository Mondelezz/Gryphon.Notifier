using Application.Features.TopicFeatures.Query;

namespace Application.Services;

public partial class EventService
{
    /// <summary>
    /// Добавляет событие к топику
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Топик</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public async Task<GetTopic.ResponseDto> AddEventToTopicAsync(long userId, long topicId, long eventId, CancellationToken cancellationToken)
    {
        Event eventDb = await eventRepository.GetEventByIdAsync(userId, topicId, cancellationToken)
             ?? throw new EntityNotFoundException(eventId, userId, nameof(eventId), nameof(userId));

        if (eventDb.IsDeleted)
        {
            throw new InvalidDataException("The event should not be deleted.");
        }

        Topic topicDb = await topicRepository.GetTopicByIdAsync(topicId, userId, cancellationToken)
                ?? throw new EntityNotFoundException(topicId, userId, nameof(topicId), nameof(userId));

        if (topicDb.IsDeleted)
        {
            throw new InvalidDataException("The topic should not be deleted.");
        }

        eventDb.TopicId = topicDb.Id;

        await eventRepository.UpdateAsync(eventDb, cancellationToken);

        return new GetTopic.ResponseDto(GetTopic.Mapper.Map(topicDb));
    }
}
