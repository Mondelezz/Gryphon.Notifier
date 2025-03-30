using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Event> Events { get; set; }
}
