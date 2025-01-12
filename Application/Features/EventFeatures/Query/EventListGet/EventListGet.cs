using Application.Common;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EventFeatures.Query.EventListGet;

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
                .Include(e => e.GroupEvent)
                .Where(e => e.UserId == request.UserId)
                .AsQueryable();

            ApplyFilters(request.Filter, ref query);

            int totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return new ResponseDto([], totalCount);
            }

            if (request.SkipCount >= totalCount)
            {
                request = request with { SkipCount = totalCount - 1 };
            }

            Sort(request.Sorting, ref query, request.SortByDescending);

            IReadOnlyList<Event> result = await query
                .Skip(request.SkipCount)
                .Take(request.Offset)
                .ToListAsync(cancellationToken);

            return new ResponseDto(Mapper.Map(result), totalCount);
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
        }

        private static void Sort(Sorting sorting, ref IQueryable<Event> query, bool sortByDescending) =>

            query = (sorting, sortByDescending) switch
            {
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
