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
                .AddInterceptors(new OutboxMessagesInterceptor())
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
            $"""
            TRUNCATE TABLE
                "{CatalogDbContext.Schema}"."outbox_messages",
                "{CatalogDbContext.Schema}"."order_lines",
                "{CatalogDbContext.Schema}"."orders",
                "{CatalogDbContext.Schema}"."promotions",
                "{CatalogDbContext.Schema}"."games"
            CASCADE;
            """,
            cancellationToken
        );
    }
}
