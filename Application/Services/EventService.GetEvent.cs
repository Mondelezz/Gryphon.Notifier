using Application.Features.EventFeatures.Query;

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
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<GetEvent.ResponseDto> GetEventAsync(long userId, long eventId, CancellationToken cancellationToken)
    {
        Event eventDb = await eventRepository.GetEventByIdAsync(userId, eventId, cancellationToken)
            ?? throw new EntityNotFoundException(eventId, userId, nameof(eventId), nameof(userId));

        return new GetEvent.ResponseDto(GetEvent.Mapper.Map(eventDb));
    }
}
