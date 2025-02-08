using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Command;

/// <summary>
/// Отвечает за создание или обновление события
/// </summary>
public static partial class CreateOrUpdateEvent
{
    /// <summary>
    /// Команда
    /// </summary>
    /// <param name="RequestDto">Модель создаваемого события</param>
    /// <param name="EventId">Идентификатор обновляемого события</param>
    /// <param name="TopicId">Идентификатор топика, в котором создаётся или обновляется событие</param>
    /// <param name="CurrentUserId">Идентификатор текущего пользователя</param>
    public record Command(
        RequestDto RequestDto,
        long? EventId,
        long? TopicId,
        string CurrentUserId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken) =>
            request.EventId is null
                ? await CreateEventAsync(request, cancellationToken)
                : await UpdateEventAsync(request, cancellationToken);

        private async ValueTask<long> CreateEventAsync(Command request, CancellationToken cancellationToken)
        {
            Event eventDb = Mapper.Map(request.RequestDto.EventDto, request.CurrentUserId, request.TopicId);

            // В случае, если событие уже прошло, то мы его помечаем как завершённое. 
            if (eventDb.DateEvent < DateTime.UtcNow)
            {
                eventDb.IsCompleted = true;
            }

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

            // Обновление полей сущности
            Mapper.Map(eventDb, request.RequestDto.EventDto, request.CurrentUserId, request.TopicId);

            // В случае, если событие уже прошло, то мы его помечаем как завершённое. 
            if (eventDb.DateEvent < DateTime.UtcNow)
            {
                eventDb.IsCompleted = true;
            }

            commandDbContext.Update(eventDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return eventDb.Id;
        }
    }
}
