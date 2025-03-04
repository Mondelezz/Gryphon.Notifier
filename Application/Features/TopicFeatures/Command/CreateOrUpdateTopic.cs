using Application.Common;
using Infrastructure.DbContexts;

using Mediator;

namespace Application.Features.TopicFeatures.Command;

/// <summary>
/// Отвечает за создание/обновление топика
/// </summary>
public static partial class CreateOrUpdateTopic
{
    public record Command(
        long CurrentUserId,
        long? TopicId,
        RequestDto RequestDto) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public async ValueTask<long> Handle(Command request, CancellationToken cancellationToken) =>
            request.TopicId is null
                ? await CreateTopicAsync(request, cancellationToken)
                : await UpdateTopicAsync(request, cancellationToken);

        private async ValueTask<long> CreateTopicAsync(Command request, CancellationToken cancellationToken)
        {
            Topic topicDb = Mapper.Map(request.RequestDto.TopicDto, request.CurrentUserId);

            await commandDbContext.Topics.AddAsync(topicDb, cancellationToken);
            await commandDbContext.SaveChangesAsync(cancellationToken);

            return topicDb.Id;
        }

        private async ValueTask<long> UpdateTopicAsync(Command request, CancellationToken cancellationToken)
        {
            Topic topicDb = await commandDbContext.Topics
                .Where(t => t.Id == request.TopicId && t.UserId == request.CurrentUserId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(request.TopicId!.Value, request.CurrentUserId, "topic", "user");

            // Обновление полей сущности
            Mapper.Map(topicDb, request.RequestDto.TopicDto);

            commandDbContext.Update(topicDb);

            await commandDbContext.SaveChangesAsync(cancellationToken);

            return topicDb.Id;
        }
    }
}
