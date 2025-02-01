using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new NullReferenceException("ConnectionString to database is null");

        NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);

        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddDbContextPool<CommandDbContext>(options =>
        {
            options.UseNpgsql(dataSource)
            .DevelopmentEnableSensitiveData(environment);
        });

        services.AddDbContextPool<QueryDbContext>(options =>
        {
            options.UseNpgsql(dataSource)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .DevelopmentEnableSensitiveData(environment);
        });

        services.AddDbContext<MigrationDbContext>(options =>
        {
            options.UseNpgsql(dataSource, opt =>
            {
                opt.CommandTimeout(1200);
                opt.MigrationsAssembly(typeof(MigrationDbContext).Assembly.GetName().Name);
            });
        });

        return services;
    }

    private static DbContextOptionsBuilder DevelopmentEnableSensitiveData(
        this DbContextOptionsBuilder optionsBuilder,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            optionsBuilder.EnableSensitiveDataLogging(true)
                          .EnableDetailedErrors();
        }

        return optionsBuilder;
    }
}
