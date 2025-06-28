namespace Application.Services;

public partial class EventService
{
    /// <summary>
    /// Получает событие
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Событие</returns>
    /// <exception cref="EntityNotFoundException">NRE</exception>
    /// <exception cref="AccessException">Ошибка доступа</exception>
    public async Task<Event> GetEventByIdAsync(long userId, long eventId, CancellationToken cancellationToken)
    {
        Event eventDb = await eventRepository.GetEventByIdAsync(eventId, cancellationToken)
            ?? throw new EntityNotFoundException(eventId, nameof(eventId));

        if (eventDb.UserId != userId)
        {
            throw new AccessException($"Ошибка доступа к событию: {eventId} для пользователя: {userId}");
        }

        return eventDb;
    }
}
