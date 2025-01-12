using Application.Common;
using Application.Exceptions;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EventFeatures.Query.EventGet;

public static partial class EventGet
{
    public record Query(
        string UserId,
        long EventId) : IQueryRequest<ResponseDto>;

    public class Handler(QueryDbContext queryDbContext) : IRequestHandler<Query, ResponseDto>
    {
        public async ValueTask<ResponseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            Event eventDb = await queryDbContext.Events
                .Where(e => e.Id == request.EventId && e.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.EventId, request.UserId, "event", "user");

            return new ResponseDto(Mapper.Map(eventDb));
        }
    }
}
