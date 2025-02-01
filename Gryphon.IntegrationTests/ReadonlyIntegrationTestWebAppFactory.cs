using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Gryphon.IntegrationTests;

public class ReadonlyIntegrationTestWebAppFactory : IntegrationTestWebAppFactory
{
    public override async Task InitContainersAsync() => await DatabaseFixture.InitializeAsync();
    public override async Task DisposeContainersAsync() => await DatabaseFixture.DisposeAsync();

    public override async Task InitDatabase(MigrationDbContext migrationDbContext)
    {
        string sqlScript = await File.ReadAllTextAsync("./dump_data.sql");

        await migrationDbContext.Database.ExecuteSqlRawAsync(sqlScript);
    }
}
