using Application.Features.EventFeatures.Query;
using Application.Features.EventFeatures.Command;
using Application.Features.TopicFeatures.Query;

namespace Application.Interfaces;

public interface IEventService
{
    /// <summary>
    /// Получает список событий
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="offset">Сколько получить элементов</param>
    /// <param name="skipCount">Сколько пропустить элементов</param>
    /// <param name="sorting">Сортировка</param>
    /// <param name="sortByDescending">Сортировка по убыванию</param>
    /// <param name="filter">Фильтры</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список событий</returns>
    public Task<GetListEvent.ResponseDto> GetListEventAsync(
            long userId,
            int offset,
            int skipCount,
            GetListEvent.Sorting sorting,
            bool sortByDescending,
            GetListEvent.RequestFilter? filter,
            CancellationToken cancellationToken);

    /// <summary>
    /// Получает событие
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Событие</returns>
    public Task<GetEvent.ResponseDto> GetEventAsync(
        long userId,
        long eventId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт или обновляет информаию о событии
    /// </summary>
    /// <param name="eventDto">Данные события</param>
    /// <param name="eventId">Идентификатор события (если обновляем событие)</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор события</returns>
    public Task<long> CreateOrUpdateEventAsync(
        CreateOrUpdateEvent.EventDto eventDto,
        long? eventId,
        long? topicId,
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет событие к топику
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Топик</returns>
    public Task<GetTopic.ResponseDto> AddEventToTopicAsync(
        long userId,
        long topicId,
        long eventId,
        CancellationToken cancellationToken);
}
