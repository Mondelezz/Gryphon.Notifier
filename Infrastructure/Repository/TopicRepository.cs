using Domain.Interfaces;
using Domain.Models;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal class TopicRepository : BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<Topic> AddTopicAsync(Topic topic, CancellationToken cancellationToken)
    {
        await AddAsync(topic, cancellationToken);

        await commandDbContext.SaveChangesAsync(cancellationToken);

        return topic;
    }

    public async Task<Topic?> GetTopicByIdAsync(long topicId, long userId, CancellationToken cancellationToken) =>
        await queryDbContext.Topics
        .Where(t => t.UserId == userId && t.Id == topicId)
        .Include(t => t.Events.OrderByDescending(e => e.UpdateDate))
        .FirstOrDefaultAsync(cancellationToken);
}
