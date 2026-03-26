using Kongroo.CloudGames.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public const string Schema = "catalog";

    public required DbSet<Game> Games { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new GameConfiguration());
    }
}
