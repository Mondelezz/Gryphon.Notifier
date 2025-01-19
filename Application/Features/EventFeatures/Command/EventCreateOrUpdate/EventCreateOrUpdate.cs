using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Command.EventCreateOrUpdate;

/// <summary>
/// Отвечает за создание или обновление события
/// </summary>
public static partial class EventCreateOrUpdate
{
    /// <summary>
    /// Команда
    /// </summary>
    /// <param name="RequestDto">Модель создаваемого события</param>
    /// <param name="EventId">Идентификатор обновляемого события</param>
    /// <param name="GroupEventId">Идентификатор группы, в которой создаётся или обновляется событие</param>
    /// <param name="CurrentUserId">Идентификатор текущего пользователя</param>
    public record Command(
        RequestDto RequestDto,
        long? EventId,
        long? GroupEventId,
        string CurrentUserId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.EventId is null)
            {
                return await CreateEventAsync(request, cancellationToken);
            }

            return await UpdateEventAsync(request, cancellationToken);
        }

        private async ValueTask<long> CreateEventAsync(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = Mapper.Map(request.RequestDto.EventDto, request.CurrentUserId, request.GroupEventId);

            await commandDbContext.Events.AddAsync(eventDb, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }

        private async ValueTask<long> UpdateEventAsync(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = await commandDbContext.Events
                .Where(e => e.Id == request.EventId && e.UserId == request.CurrentUserId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.EventId!.Value, request.CurrentUserId, "event", "user");

            eventDb = Mapper.Map(request.RequestDto.EventDto, request.CurrentUserId, request.GroupEventId);

            commandDbContext.Update(eventDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }
    }
}
