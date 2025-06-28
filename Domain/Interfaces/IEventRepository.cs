using Domain.Models;

namespace Domain.Interfaces;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event> AddEventAsync(Event eventDb, CancellationToken cancellationToken);

    Task<Event?> GetEventByIdAsync(long eventId, CancellationToken cancellationToken);
}
