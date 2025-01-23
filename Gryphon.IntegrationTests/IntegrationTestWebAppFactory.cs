using Infrastructure.DbContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gryphon.IntegrationTests;

public abstract class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    /// <summary>
    /// Строка подключения для базы данных
    /// </summary>
    public string? ConnectionString { get; private set; }

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

        BaseDbContext context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        await context.Database.EnsureCreatedAsync();

        // Применение всех миграций к базе данных
        await context.Database.MigrateAsync();

        // Инициализация и заполнение базы данных тестовыми данными
        await InitDatabase(context);
    }

    async Task IAsyncLifetime.DisposeAsync() => await DisposeContainersAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            ServiceDescriptor? dbContextServiceDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(DbContextOptions<BaseDbContext>));

            if (dbContextServiceDescriptor is not null)
            {
                // Удалить фактическую регистрацию dbcontext
                services.Remove(dbContextServiceDescriptor);
            }

            // Регистрация dbcontext со строкой подключения контейнера
            services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(ConnectionString));
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
    /// <param name="context">context</param>
    public abstract Task InitDatabase(BaseDbContext context);
}
