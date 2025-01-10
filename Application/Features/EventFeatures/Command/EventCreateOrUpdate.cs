using Application.Common;
using Application.Exceptions;
using Domain.Models.Event;
using Infrastructure.DbContexts;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EventFeatures.Command;

/// <summary>
/// Отвечает за создание или обновление события
/// </summary>
public static partial class EventCreateOrUpdate
{
    public record Command(
        RequestDto RequestDto,
        long? EventId,
        string CurrentUserId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.EventId is null)
            {
                return await CreateEventAsync(request, cancellationToken);
            }
            else
            {
                return await UpdateEventAsync(request, cancellationToken);
            }
        }

        private async ValueTask<long> CreateEventAsync(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = Mapper.MapEventDtoToEvent(request.RequestDto.EventDto, request.CurrentUserId);

            await commandDbContext.Events.AddAsync(eventDb, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }

        private async ValueTask<long> UpdateEventAsync(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = await commandDbContext.Events
                .Where(e => e.Id == request.EventId && e.UserId == request.CurrentUserId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.EventId!.Value, "Event");

            commandDbContext.Update(eventDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }
    }
}
