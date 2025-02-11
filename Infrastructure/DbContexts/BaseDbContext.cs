using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Event> Events { get; set; }
}
