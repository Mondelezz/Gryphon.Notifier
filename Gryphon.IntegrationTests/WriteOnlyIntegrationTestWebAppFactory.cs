using Infrastructure.DbContexts;

namespace Gryphon.IntegrationTests;

public class WriteOnlyIntegrationTestWebAppFactory : IntegrationTestWebAppFactory
{
    public override async Task DisposeContainersAsync() => await DatabaseFixture.DisposeAsync();
    public override async Task InitContainersAsync() => await DatabaseFixture.InitializeAsync();
    public override async Task InitDatabase(MigrationDbContext migrationDbContext) => await InitializeAsync();
}
