using Domain.Enums;
using Domain.Models.Event;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<GroupEvent> GroupEvents { get; set; }
    public DbSet<Event> Events { get; set; }

    public static void MapEnums(
    ModelBuilder? modelBuilder = default,
    NpgsqlDataSourceBuilder? npgsqlDataSourceBuilder = default)
    {
        MapEnum<Importance>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<GroupEventType>(modelBuilder, npgsqlDataSourceBuilder);
    }
}
