using Domain.Common;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using NullReferenceException = Application.Exceptions.NullReferenceException;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        MapEnums(modelBuilder);
        EntityDateConfigure(modelBuilder);

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

    private static void MapEnum<TEnum>(
       ModelBuilder? modelBuilder = default,
       NpgsqlDataSourceBuilder? npgsqlDataSourceBuilder = default)
       where TEnum : struct, Enum
    {
        modelBuilder?.HasPostgresEnum<TEnum>();

        npgsqlDataSourceBuilder?.MapEnum<TEnum>();
    }
}
