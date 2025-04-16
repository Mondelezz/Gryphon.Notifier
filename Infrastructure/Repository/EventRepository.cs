using Domain.Interfaces;
using Domain.Models;

using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

internal class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<Event> AddEventAsync(Event eventDb, CancellationToken cancellationToken)
    {
        await AddAsync(eventDb, cancellationToken);

        await commandDbContext.SaveChangesAsync(cancellationToken);

        return eventDb;
    }

    public async Task<Event?> GetEventByIdAsync(long userId, long eventId, CancellationToken cancellationToken) =>
        await queryDbContext.Events
            .Where(e => e.UserId == userId && e.Id == eventId)
            .Include(e => e.Topic)
            .FirstOrDefaultAsync(cancellationToken);
}
