using Kongroo.CloudGames.Library.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Library.Infrastructure;

public sealed class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public const string Schema = "library";

    public DbSet<GameOwnership> GameOwnerships => Set<GameOwnership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new GameOwnershipConfiguration());
    }
}
