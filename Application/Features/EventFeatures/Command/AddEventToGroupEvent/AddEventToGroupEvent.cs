using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Command.AddEventToGroupEvent;

/// <summary>
/// Отвечает за добавление события к какой-либо группе
/// </summary>
public static partial class AddEventToGroupEvent
{
    public record Command(
        string CurrentUserId,
        long GroupEventId,
        long EventId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            GroupEvent groupEventDb = await GetGroupEventAsync(request, cancellationToken);

            Event eventDb = await GetEventAsync(request, cancellationToken);

            eventDb.GroupEventId = groupEventDb.Id;

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


        private async Task<GroupEvent> GetGroupEventAsync(Command request, CancellationToken cancellationToken) =>
            await commandDbContext.GroupEvents
                .Where(ge => ge.Id == request.GroupEventId &&
                             ge.UserId == request.CurrentUserId &&
                            !ge.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.GroupEventId, request.CurrentUserId, "groupEvent", "user");
    }
}
