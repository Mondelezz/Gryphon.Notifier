using Application.Features.EventFeatures.Command;
using Application.Features.EventFeatures.Query;
using Application.Interfaces;

using Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Базовый класс сервиса, содержащий приватные методы, используемые в частичных классах сервиса
/// </summary>
/// <param name="eventRepository">Репозиторий событий</param>
/// <param name="topicRepository">Репозиторий топиков</param>
public partial class EventService(
    IEventRepository eventRepository,
    ITopicRepository topicRepository) : IEventService
{
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
