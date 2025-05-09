using Application.Features.EventFeatures.Command;

namespace Application.Services;

public partial class EventService
{
    /// <summary>
    /// Создаёт или обновляет информаию о событии
    /// </summary>
    /// <param name="eventDto">Данные события</param>
    /// <param name="eventId">Идентификатор события (если обновляем событие)</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор события</returns>
    public async Task<long> CreateOrUpdateEventAsync(
        CreateOrUpdateEvent.EventDto eventDto,
        long? eventId,
        long? topicId,
        long userId,
        CancellationToken cancellationToken)
    {
        if (eventId is null)
        {
            return await CreateEventAsync(eventDto, userId, topicId, cancellationToken);
        }

        return await UpdateEventAsync(userId, eventId.Value, topicId, eventDto, cancellationToken);
    }
}
