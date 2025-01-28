using Testcontainers.PostgreSql;

namespace Gryphon.IntegrationTests;

public class DatabaseFixture : IAsyncLifetime
{
    internal readonly PostgreSqlContainer PostgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16.3-alpine")
            .WithDatabase("GryphonNotifier")
            .WithUsername("postgres")
            .WithPassword("26032005")
            .Build();

    public Task InitializeAsync() => PostgresContainer.StartAsync();

    public Task DisposeAsync() => PostgresContainer.StopAsync();
}
