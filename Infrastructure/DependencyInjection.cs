using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Application.Options;
using Minio;

using Npgsql;
using Application.Interfaces;
using Infrastructure.Repository;
using Domain.Interfaces;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.Configure<S3Options>(
            configuration.GetSection("S3Options"));

        S3Options s3Options = configuration
                .GetSection(nameof(S3Options))
                .Get<S3Options>() ?? throw new ArgumentException(nameof(S3Options));

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

        services.AddMinio(opt => opt
           .WithEndpoint(s3Options.Endpoint)
           .WithSSL(false)
           .WithCredentials(s3Options.AccessKey, s3Options.SecretKey));

        services.AddScoped<IFileDataRepository, FileDataRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();

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
