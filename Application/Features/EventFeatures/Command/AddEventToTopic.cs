using Application.Common;
using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Command;

/// <summary>
/// Отвечает за добавление события в топик
/// </summary>
public static class AddEventToTopic
{
    public record Command(
        long CurrentUserId,
        long TopicId,
        long EventId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            Topic topicDb = await GetTopicAsync(request, cancellationToken);

            Event eventDb = await GetEventAsync(request, cancellationToken);

            eventDb.TopicId = topicDb.Id;

            commandDbContext.Update(eventDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }

        private async Task<Event> GetEventAsync(Command request, CancellationToken cancellationToken) =>
            await commandDbContext.Events
                .Where(e => e.Id == request.EventId &&
                            e.UserId == request.CurrentUserId &&
                           !e.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.EventId, request.CurrentUserId, "event", "user");


        private async Task<Topic> GetTopicAsync(Command request, CancellationToken cancellationToken) =>
            await commandDbContext.Topics
                .Where(t => t.Id == request.TopicId &&
                             t.UserId == request.CurrentUserId &&
                            !t.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.TopicId, request.CurrentUserId, "topic", "user");
    }
}
