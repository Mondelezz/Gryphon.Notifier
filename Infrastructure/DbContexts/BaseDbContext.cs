using Domain.Models.Event;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<GroupEvent> GroupEvents { get; set; }
    public DbSet<Event> Events { get; set; }
}
