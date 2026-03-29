using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.IntegrationTests.Catalog;

public sealed class CatalogTestDatabase(PostgreSqlFixture fixture)
{
    public CatalogDbContext CreateDbContext() =>
        new(
            new DbContextOptionsBuilder<CatalogDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    fixture.ConnectionString,
                    postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", CatalogDbContext.Schema)
                )
                .UseSnakeCaseNamingConvention()
                .Options
        );

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync(cancellationToken);
        await context.Database.ExecuteSqlRawAsync(
            $"""TRUNCATE TABLE "{CatalogDbContext.Schema}"."games";""",
            cancellationToken
        );
    }
}
