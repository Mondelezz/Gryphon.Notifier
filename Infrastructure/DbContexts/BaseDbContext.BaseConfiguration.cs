using Domain.Common;

using Domain.Models.Event;

using Infrastructure.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EntityDateConfigure(modelBuilder);

        modelBuilder.Entity<Event>()
            .Property(e => e.Importance)
            .HasConversion<string>();

        modelBuilder.Entity<GroupEvent>()
            .Property(e => e.GroupEventType)
            .HasConversion<string>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseConfiguration<>).Assembly);
    }

    private static void EntityDateConfigure(ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableEntityType> types = modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(IEntityDate)));

        foreach (IMutableEntityType entity in types)
        {
            IMutableProperty property = entity.FindProperty("CreateDate")
                ?? throw new NullReferenceException("CreateDate is null");

            property.SetDefaultValueSql("timezone('utc', current_timestamp)");

            property = entity.FindProperty("UpdateDate")
                ?? throw new NullReferenceException("UpdateDate is null");

            property.SetDefaultValueSql("timezone('utc', current_timestamp)");
        }
    }
}
