using Application.Common;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EventFeatures.Query.GroupEventListGet;

public static partial class GroupEventListGet
{
    public record Query(
        string CurrentUserId) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            IReadOnlyList<GroupEvent> groupEventsDb = await queryDbContext.GroupEvents
                .Where(ge => ge.UserId == request.CurrentUserId)
                .OrderBy(ge => ge.CreateDate)
                .ToListAsync(cancellationToken);

            int totalCount = groupEventsDb.Count;

            if (totalCount == 0)
            {
                return new ResponseDto([], 0);
            }

            return new ResponseDto(Mapper.Map(groupEventsDb), totalCount);
        }
    }
}
