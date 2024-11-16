using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public sealed class MigrationDbContext(DbContextOptions<MigrationDbContext> contextOptions)
    : BaseDbContext(contextOptions)
{
}
