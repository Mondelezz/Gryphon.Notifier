using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Query;

/// <summary>
/// Отвечает за получение списка топиков
/// </summary>
public static partial class GetListTopic
{
    public record Query(
        string CurrentUserId) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Topic> topicDb = await queryDbContext.Topics
                .Where(ge => ge.UserId == request.CurrentUserId && !ge.IsDeleted)
                .OrderBy(ge => ge.CreateDate)
                .ToListAsync(cancellationToken);

            int totalCount = topicDb.Count;

            if (totalCount == 0)
            {
                return new ResponseDto([], totalCount);
            }

            return new ResponseDto(Mapper.Map(topicDb), totalCount);
        }
    }
}
