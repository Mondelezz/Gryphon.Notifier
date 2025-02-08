using Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Gryphon.IntegrationTests;

public class ReadonlyIntegrationTestWebAppFactory : IntegrationTestWebAppFactory
{
    public override async Task InitContainersAsync() => await DatabaseFixture.InitializeAsync();
    public override async Task DisposeContainersAsync() => await DatabaseFixture.DisposeAsync();

    private const string Backup = "./dump_data.sql";

    public override async Task InitDatabase(MigrationDbContext migrationDbContext)
    {
        string sqlScript = await File.ReadAllTextAsync(Backup);

        await migrationDbContext.Database.ExecuteSqlRawAsync(sqlScript);
    }
}
