using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.TopicFeatures.Query;

/// <summary>
/// Отвечает за получение топика и входящих в него событий
/// </summary>
public static partial class GetTopic
{
    public record Query(
        string UserId,
        long TopicId) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            Topic topicDb = await queryDbContext.Topics
                .Where(t => t.Id == request.TopicId && t.UserId == request.UserId)
                .Include(t => t.Events.OrderByDescending(e => e.UpdateDate))
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.TopicId, request.UserId, "topicId", "userId");

            return new ResponseDto(Mapper.Map(topicDb));
        }
    }
}
