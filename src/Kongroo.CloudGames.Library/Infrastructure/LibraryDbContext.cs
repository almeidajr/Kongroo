using Kongroo.BuildingBlocks.Infrastructure;
using Kongroo.CloudGames.Library.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Library.Infrastructure;

public sealed class LibraryDbContext(DbContextOptions<LibraryDbContext> options)
    : OutboxDbContext<LibraryDbContext>(options),
        IRelationalDbContext
{
    public static string Schema => "library";

    public DbSet<GameOwnership> GameOwnerships => Set<GameOwnership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new GameOwnershipConfiguration());
    }
}
