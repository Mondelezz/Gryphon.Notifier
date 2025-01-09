using Application.Common;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;

namespace Application.Features.EventFeatures.Command;

/// <summary>
/// Отвечает за создание события
/// </summary>
public static partial class EventCreate
{
    public record Command(
        RequestDto RequestDto,
        string CurrentUserId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = Mapper.MapEventDtoToEvent(
                request.RequestDto.EventDto,
                request.CurrentUserId);

            await commandDbContext.Events.AddAsync(eventDb, cancellationToken);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }
    }
}
