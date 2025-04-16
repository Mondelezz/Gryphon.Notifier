using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<Topic> Topics { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<FileData> FileDatas { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserLogin> UserLogins { get; set; }

    public DbSet<UserToken> UserTokens { get; set; }
}
