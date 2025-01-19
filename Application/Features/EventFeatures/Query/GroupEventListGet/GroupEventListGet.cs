using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Query.GroupEventListGet;

/// <summary>
/// Отвечает за получение списка групп для событий
/// </summary>
public static partial class GroupEventListGet
{
    public record Query(
        string CurrentUserId) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            IReadOnlyList<GroupEvent> groupEventsDb = await queryDbContext.GroupEvents
                .Where(ge => ge.UserId == request.CurrentUserId && !ge.IsDeleted)
                .OrderBy(ge => ge.CreateDate)
                .ToListAsync(cancellationToken);

            int totalCount = groupEventsDb.Count;

            if (totalCount == 0)
            {
                return new ResponseDto([], totalCount);
            }

            return new ResponseDto(Mapper.Map(groupEventsDb), totalCount);
        }
    }
}
