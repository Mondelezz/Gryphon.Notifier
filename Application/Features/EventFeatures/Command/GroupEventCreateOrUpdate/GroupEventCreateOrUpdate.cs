using Application.Common;

using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.EventFeatures.Command.GroupEventCreateOrUpdate;

/// <summary>
/// Отвечает за создание/обновление группы для событий
/// </summary>
public static partial class GroupEventCreateOrUpdate
{
    public record Command(
        string CurrentUserId,
        long? GroupId,
        RequestDto RequestDto) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.GroupId is null)
            {
                return await CreateGroupEventAsync(request, cancellationToken);
            }

            return await UpdateGroupEventAsync(request, cancellationToken);
        }

        private async ValueTask<long> CreateGroupEventAsync(Command request, CancellationToken cancellationToken)
        {
            GroupEvent groupEventDb = Mapper.Map(request.RequestDto.GroupEventDto, request.CurrentUserId);

            await commandDbContext.GroupEvents.AddAsync(groupEventDb, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);

            return groupEventDb.Id;
        }

        private async ValueTask<long> UpdateGroupEventAsync(Command request, CancellationToken cancellationToken)
        {
            GroupEvent groupEventDb = await commandDbContext.GroupEvents
                .Where(ge => ge.Id == request.GroupId && ge.UserId == request.CurrentUserId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.GroupId!.Value, request.CurrentUserId, "groupEvent", "user");

            // Обновление полей сущности
            Mapper.Map(groupEventDb, request.RequestDto.GroupEventDto);

            commandDbContext.Update(groupEventDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return groupEventDb.Id;
        }
    }
}
