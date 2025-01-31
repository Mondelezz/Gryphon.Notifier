using Testcontainers.PostgreSql;

namespace Gryphon.IntegrationTests;

/// <summary>
/// Отвечвает за инициализацию и уничтожение PostgreSqlContainer
/// </summary>
public class DatabaseFixture : IAsyncLifetime
{
    internal readonly PostgreSqlContainer PostgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16.3-alpine")
            .WithUsername("postgres")
            .WithPassword("26032005")
            .Build();

    public async Task InitializeAsync() => await PostgresContainer.StartAsync();

    public async Task DisposeAsync() => await PostgresContainer.StopAsync();
}
