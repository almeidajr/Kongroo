using Kongroo.BuildingBlocks.Infrastructure;
using Kongroo.CloudGames.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Infrastructure;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : OutboxDbContext<IdentityDbContext>(options),
        IRelationalDbContext
{
    public static string Schema => "identity";

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
