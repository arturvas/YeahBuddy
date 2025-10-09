using Microsoft.EntityFrameworkCore;
using YeahBuddy.Domain.Entities;

namespace YeahBuddy.Infrastructure.DataAccess;

public class YeahBuddyDbContext(DbContextOptions<YeahBuddyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YeahBuddyDbContext).Assembly);
    }
}