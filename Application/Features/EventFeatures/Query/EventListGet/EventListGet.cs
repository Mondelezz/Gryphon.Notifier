using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Query.EventListGet;

/// <summary>
/// Отвечает за получение списка событий с возможностью фильтрации, сортировки и пагинации
/// в том числе идёт получение общего количества событий, количества активных и завершённых событий
/// </summary>
public static partial class EventListGet
{
    public record Query(
            string UserId,
            int Offset,
            int SkipCount,
            Sorting Sorting,
            bool SortByDescending,
            RequestFilter? Filter) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Event> query = queryDbContext.Events
                .Where(e => e.UserId == request.UserId)
                .Include(e => e.GroupEvent)
                .AsQueryable();

            ApplyFilters(request.Filter, ref query);

            int totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return new ResponseDto([], 0, 0, 0, 0);
            }

            if (request.SkipCount >= totalCount)
            {
                request = request with { SkipCount = totalCount - 1 };
            }

            decimal totalPrice = await query.SumAsync(e => e.Price, cancellationToken) ?? 0;

            int actualEventsCount = await query.CountAsync(e => e.DateEvent > DateTime.UtcNow, cancellationToken);

            int endedEventsCount = totalCount - actualEventsCount;

            Sort(request.Sorting, ref query, request.SortByDescending);

            IReadOnlyList<Event> result = await query
                .Skip(request.SkipCount)
                .Take(request.Offset)
                .ToListAsync(cancellationToken);

            return new ResponseDto(Mapper.Map(result), totalCount, actualEventsCount, endedEventsCount, totalPrice);
        }

        private static void ApplyFilters(
            RequestFilter? filter,
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
                    EF.Functions.ILike(e.Name, $"%{filter.SearchTermFilter}%") ||
                    EF.Functions.ILike(e.Description ?? "", $"%{filter.SearchTermFilter}%"));
            }

            if (filter.GroupEventId is not null)
            {
                query = query.Where(e => e.GroupEventId == filter.GroupEventId.Value);
            }
        }

        private static void Sort(Sorting sorting, ref IQueryable<Event> query, bool sortByDescending) =>

            query = (sorting, sortByDescending) switch
            {
                (Sorting.Id, true) => query.OrderByDescending(e => e.Id),
                (Sorting.Id, false) => query.OrderBy(e => e.Id),

                (Sorting.DateEvent, true) => query.OrderByDescending(e => e.DateEvent),
                (Sorting.DateEvent, false) => query.OrderBy(e => e.DateEvent),

                (Sorting.Importance, true) => query.OrderByDescending(e => e.Importance),
                (Sorting.Importance, false) => query.OrderBy(e => e.Importance),

                (Sorting.Price, true) => query.OrderByDescending(e => e.Price.HasValue).ThenByDescending(e => e.Price),
                (Sorting.Price, false) => query.OrderBy(e => e.Price.HasValue).ThenBy(e => e.Price),

                _ => query.OrderByDescending(e => e.CreateDate) // default
            };
    }
}
