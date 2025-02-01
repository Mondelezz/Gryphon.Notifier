using Infrastructure.DbContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests;

public abstract class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    /// <summary>
    /// Строка подключения для базы данных
    /// </summary>
    public string? ConnectionString { get; private set; }

    private const string TestEnv = "Test";

    private const string ParamsConnectionString = ";Include Error Detail=true;";

    internal readonly DatabaseFixture DatabaseFixture = new();

    /// <summary>
    /// Запускает контейнеры, накатывает миграции и инициализирует базу данных.
    /// </summary>
    /// <returns>Task</returns>
    public async Task InitializeAsync()
    {
        await InitContainersAsync();

        ConnectionString = DatabaseFixture.PostgresContainer.GetConnectionString() + ParamsConnectionString;

        using IServiceScope scope = Services.CreateScope();

        MigrationDbContext migrationDbContext = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();

        // Получает все миграции, определённые в сборке, но не применённые к тестовой базе данных.
        IEnumerable<string>? pendingMigrations = await migrationDbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await migrationDbContext.Database.MigrateAsync();

            await InitDatabase(migrationDbContext);
        }
    }

    /// <summary>
    /// Переопределяет строку подключения к базе данных для тестового контейнера
    /// </summary>
    /// <param name="builder">Построитель</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder
        .UseEnvironment(TestEnv)
        .UseConfiguration(new ConfigurationBuilder().AddJsonFile($"appsettings.{TestEnv}.json").Build())
        .ConfigureTestServices(services =>
        {
            List<ServiceDescriptor>? dbContextDescriptors = services
                .Where(s => s.ServiceType == typeof(DbContextOptions<QueryDbContext>) ||
                            s.ServiceType == typeof(DbContextOptions<CommandDbContext>) ||
                            s.ServiceType == typeof(DbContextOptions<MigrationDbContext>))
                .ToList();

            foreach (ServiceDescriptor descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<QueryDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            services.AddDbContext<CommandDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            services.AddDbContext<MigrationDbContext>(options =>
                options.UseNpgsql(ConnectionString));
        });

    /// <summary>
    /// Уничтожает ресурсы тестового контейнера
    /// </summary>
    async Task IAsyncLifetime.DisposeAsync() => await DisposeContainersAsync();

    /// <summary>
    /// Инициализация контейнеров для тестов
    /// </summary>
    public abstract Task InitContainersAsync();

    /// <summary>
    /// Утилизировать ресурсы контейнеров для тестов
    /// </summary>
    public abstract Task DisposeContainersAsync();

    /// <summary>
    /// Инициализация и заполнения базы данных
    /// </summary>
    /// <param name="migrationDbContext">Контекст для миграций</param>
    public abstract Task InitDatabase(MigrationDbContext migrationDbContext);
}
