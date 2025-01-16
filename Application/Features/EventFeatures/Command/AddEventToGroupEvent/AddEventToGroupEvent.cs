using Application.Common;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;


namespace Application.Features.EventFeatures.Command.AddEventToGroupEvent;

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
        }

        private async Task<GroupEvent> GetGroupEventAsync(Command request, CancellationToken cancellationToken)
        {
            GroupEvent groupEventDb = await commandDbContext.GroupEvents
                .Where(ge => ge.Id == request.GroupEventId &&
                             ge.UserId == request.CurrentUserId &&
                            !ge.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.GroupEventId, "groupEvent");

            return groupEventDb;
        }
    }
}
