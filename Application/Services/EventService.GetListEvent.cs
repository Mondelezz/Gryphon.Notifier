using Application.Features.EventFeatures.Query;

namespace Application.Services;

public partial class EventService
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
}
