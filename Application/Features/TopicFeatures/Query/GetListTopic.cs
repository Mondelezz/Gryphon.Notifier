using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.TopicFeatures.Query;

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
                .Where(t => t.UserId == request.CurrentUserId && !t.IsDeleted)
                .OrderBy(t => t.CreateDate)
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
