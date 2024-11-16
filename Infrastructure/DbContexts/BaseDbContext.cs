using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<AutoInsuranceProduct> AutoInsuranceProducts { get; set; }


    public static void MapEnums(
    ModelBuilder? modelBuilder = default,
    NpgsqlDataSourceBuilder? npgsqlDataSourceBuilder = default)
    {
        MapEnum<State>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<BrandAuto>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<AvailableBonusesAndDiscounts>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<CoveredRisk>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<InsuranceType>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<VehicleInsuranceClass>(modelBuilder, npgsqlDataSourceBuilder);
        MapEnum<VehicleUsageType>(modelBuilder, npgsqlDataSourceBuilder);
    }
}
