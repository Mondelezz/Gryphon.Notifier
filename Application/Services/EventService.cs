using Application.Features.EventFeatures.Command;
using Application.Features.EventFeatures.Query;
using Application.Features.TopicFeatures.Query;
using Application.Interfaces;

using Domain.Interfaces;

namespace Application.Services;

public class EventService(
    IEventRepository eventRepository,
    ITopicRepository topicRepository) : IEventService
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
    public async Task<GetListEvent.ResponseDto> GetListEventAsync(
        long userId,
        int offset,
        int skipCount,
        GetListEvent.Sorting sorting,
        bool sortByDescending,
        GetListEvent.RequestFilter? filter,
        CancellationToken cancellationToken)
    {
        IQueryable<Event> query = eventRepository
            .GetAll()
            .Where(e => e.UserId == userId && !e.IsDeleted)
            .Include(e => e.Topic);

        ApplyFilters(filter, ref query);

        int totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new GetListEvent.ResponseDto([], 0, 0, 0, 0);
        }

        if (skipCount >= totalCount)
        {
            skipCount = totalCount - 1;
        }

        decimal totalPrice = await query.SumAsync(e => e.Price, cancellationToken) ?? 0;

        int actualEventsCount = await query.CountAsync(e => !e.IsCompleted, cancellationToken);

        int endedEventsCount = totalCount - actualEventsCount;
        Sort(sorting, ref query, sortByDescending);

        IReadOnlyList<Event> result = await query
            .Skip(skipCount)
            .Take(offset)
            .ToListAsync(cancellationToken);

        return new GetListEvent.ResponseDto(GetListEvent.Mapper.Map(result), totalCount, actualEventsCount, endedEventsCount, totalPrice);
    }

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

        Topic topicDb = await topicRepository.GetTopicByIdAsync(userId, topicId, cancellationToken)
                ?? throw new EntityNotFoundException(topicId, userId, nameof(topicId), nameof(userId));

        if (topicDb.IsDeleted)
        {
            throw new InvalidDataException("The topic should not be deleted.");
        }

        eventDb.TopicId = topicDb.Id;

        await eventRepository.UpdateAsync(eventDb, cancellationToken);

        return new GetTopic.ResponseDto(GetTopic.Mapper.Map(topicDb));
    }

    private static void ApplyFilters(
            GetListEvent.RequestFilter? filter,
            ref IQueryable<Event> query)
    {
        if (filter is null)
        {
            return;
        }

        if (filter.IndicatedPriceFilter.HasValue)
        {
            query = filter.IndicatedPriceFilter.Value
                ? query.Where(e => e.Price > 0) // Фильтр для событий с указанной ценой
                : query.Where(e => e.Price == null || e.Price == 0); // Фильтр для событий без указанной цены
        }

        if (!string.IsNullOrEmpty(filter.SearchTermFilter))
        {
            query = query.Where(e =>
                EF.Functions.Like(e.Name, $"%{filter.SearchTermFilter}%") ||
                EF.Functions.Like(e.Description ?? "", $"%{filter.SearchTermFilter}%"));
        }

        if (filter.TopicId is not null)
        {
            query = query.Where(e => e.TopicId == filter.TopicId.Value);
        }

        if (filter.IsCompleted is not null)
        {
            query = query.Where(e => e.IsCompleted == filter.IsCompleted);
        }
    }

    private static void Sort(GetListEvent.Sorting sorting, ref IQueryable<Event> query, bool sortByDescending) =>

        query = (sorting, sortByDescending) switch
        {
            (GetListEvent.Sorting.Id, true) => query.OrderByDescending(e => e.Id),
            (GetListEvent.Sorting.Id, false) => query.OrderBy(e => e.Id),

            (GetListEvent.Sorting.DateEvent, true) => query.OrderByDescending(e => e.DateEvent),
            (GetListEvent.Sorting.DateEvent, false) => query.OrderBy(e => e.DateEvent),

            (GetListEvent.Sorting.Importance, true) => query.OrderByDescending(e => e.Importance),
            (GetListEvent.Sorting.Importance, false) => query.OrderBy(e => e.Importance),

            (GetListEvent.Sorting.Price, true) => query.OrderByDescending(e => e.Price.HasValue).ThenByDescending(e => e.Price),
            (GetListEvent.Sorting.Price, false) => query.OrderBy(e => e.Price.HasValue).ThenBy(e => e.Price),

            _ => query.OrderByDescending(e => e.CreateDate) // default
        };

    private async Task<long> CreateEventAsync(CreateOrUpdateEvent.EventDto eventDto, long userId, long? topicId, CancellationToken cancellationToken)
    {
        Event eventDb = CreateOrUpdateEvent.Mapper.Map(eventDto, userId, topicId);

        // В случае, если событие уже прошло, то мы его помечаем как завершённое. 
        if (eventDb.DateEvent < DateTime.UtcNow)
        {
            eventDb.IsCompleted = true;
        }

        await eventRepository.AddEventAsync(eventDb, cancellationToken);

        return eventDb.Id;
    }

    private async Task<long> UpdateEventAsync(long userId, long eventId, long? topicId, CreateOrUpdateEvent.EventDto eventDto, CancellationToken cancellationToken)
    {
        Event eventDb = await eventRepository.GetEventByIdAsync(userId, eventId, cancellationToken)
            ?? throw new EntityNotFoundException(eventId, userId, nameof(eventId), nameof(userId));

        // Обновление полей сущности
        CreateOrUpdateEvent.Mapper.Map(eventDb, eventDto, userId, topicId);

        // В случае, если событие уже прошло, то мы его помечаем как завершённое. 
        eventDb.IsCompleted = eventDb.DateEvent < DateTime.UtcNow;

        await eventRepository.UpdateAsync(eventDb, cancellationToken);

        return eventDb.Id;
    }
}
