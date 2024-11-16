using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public sealed class QueryDbContext(DbContextOptions<QueryDbContext> contextOptions)
    : BaseDbContext(contextOptions)
{
}
