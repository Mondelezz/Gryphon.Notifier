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

    internal readonly DatabaseFixture DatabaseFixture = new();

    /// <summary>
    /// Запускает контейнеры, накатывает миграции и инициализирует базу данных.
    /// </summary>
    /// <returns>Task</returns>
    public async Task InitializeAsync()
    {
        await InitContainersAsync();

        ConnectionString = DatabaseFixture.PostgresContainer.GetConnectionString();

        using IServiceScope scope = Services.CreateScope();

        // Применение миграций для QueryDbContext
        QueryDbContext queryContext = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
        await queryContext.Database.MigrateAsync();

        // Применение миграций для CommandDbContext
        CommandDbContext commandContext = scope.ServiceProvider.GetRequiredService<CommandDbContext>();
        await commandContext.Database.MigrateAsync();

        // Инициализация и заполнение базы данных тестовыми данными
        await InitDatabase(queryContext, commandContext);
    }

    /// <summary>
    /// Уничтожает ресурсы тестового контейнера
    /// </summary>
    async Task IAsyncLifetime.DisposeAsync() => await DisposeContainersAsync();

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
            // Удаляем существующие регистрации контекстов, если они есть
            List<ServiceDescriptor>? dbContextDescriptors = services
                .Where(s => s.ServiceType == typeof(DbContextOptions<QueryDbContext>) ||
                            s.ServiceType == typeof(DbContextOptions<CommandDbContext>))
                .ToList();

            foreach (ServiceDescriptor descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            // Регистрируем QueryDbContext
            services.AddDbContext<QueryDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            // Регистрируем CommandDbContext
            services.AddDbContext<CommandDbContext>(options =>
                options.UseNpgsql(ConnectionString));
        });

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
    /// <param name="queryContext">Контекст запросов</param>
    /// <param name="commandContext">Контекст комманд</param>
    public abstract Task InitDatabase(QueryDbContext queryContext, CommandDbContext commandContext);
}
