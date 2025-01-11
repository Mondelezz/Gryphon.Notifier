using Application.Common;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EventFeatures.Query;

public static partial class EventListGet
{
    public record Query(
            string UserId,
            int Offset,
            Sorting Sorting,
            bool SortByDescending,
            RequestFilter? Filter) : IQueryRequest<ResponseDto>
    {
        public int SkipCount { get; set; }
    }

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
                request.SkipCount = totalCount - 1;
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

            if (filter.PriceFilter is not null)
            {
                query = query.Where(e => e.Price > 0);
            }

            if (filter.NameFilter is not null)
            {
                query = query.Where(e => e.Name == filter.NameFilter);
            }

            if (filter.DateEventFilter is not null)
            {
                query = query.Where(e => e.DateEvent == filter.DateEventFilter);
            }

            if (filter.SearchTermFilter is not null)
            {
                query = query.Where(e =>
                    EF.Functions.ILike(e.Name, $"%{filter.SearchTermFilter}%") ||
                    EF.Functions.ILike(e.Description ?? "", $"%{filter.SearchTermFilter}%"));
            }
        }

        private static void Sort(
            Sorting sorting,
            ref IQueryable<Event> query,
            bool SortByDescending)
        {
            switch (sorting)
            {
                case Sorting.DateEvent when SortByDescending:
                    query = query.OrderByDescending(e => e.DateEvent);
                    break;
                case Sorting.DateEvent:
                    query = query.OrderBy(e => e.DateEvent);
                    break;

                case Sorting.Importance when SortByDescending:
                    query = query.OrderByDescending(e => e.Importance);
                    break;
                case Sorting.Importance:
                    query = query.OrderBy(e => e.Importance);
                    break;

                case Sorting.Price when SortByDescending:
                    query = query.OrderByDescending(e => e.Price);
                    break;
                case Sorting.Price when SortByDescending:
                    query = query.OrderBy(e => e.Price);
                    break;
                default:
                    query = query.OrderByDescending(e => e.CreateDate);
                    break;
            }
        }
    }
}
