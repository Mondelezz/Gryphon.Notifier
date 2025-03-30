using Domain.Common;
using Domain.Models;

using Infrastructure.Configuration;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext(DbContextOptions options) : IdentityDbContext<User, IdentityRole<long>, long>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        EntityDateConfigure(builder);

        builder.Entity<Event>()
            .Property(e => e.Importance)
            .HasConversion<string>();

        builder.Entity<Topic>()
            .Property(e => e.TopicType)
            .HasConversion<string>();

        builder.ApplyConfigurationsFromAssembly(typeof(BaseConfiguration<>).Assembly);
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
