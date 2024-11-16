using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<InsuranceProduct> InsuranceProduct { get; set; }


    public static void MapEnums(
    ModelBuilder? modelBuilder = default,
    NpgsqlDataSourceBuilder? npgsqlDataSourceBuilder = default) => MapEnum<State>(modelBuilder, npgsqlDataSourceBuilder);
}
