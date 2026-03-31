using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Kongroo.CloudGames.Library.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.IntegrationTests.Library;

public sealed class LibraryTestDatabase(PostgreSqlFixture fixture)
{
    public LibraryDbContext CreateDbContext() =>
        new(
            new DbContextOptionsBuilder<LibraryDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    fixture.ConnectionString,
                    postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", LibraryDbContext.Schema)
                )
                .UseSnakeCaseNamingConvention()
                .Options
        );

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync(cancellationToken);
        await context.Database.ExecuteSqlRawAsync(
            $"""TRUNCATE TABLE "{LibraryDbContext.Schema}"."game_ownerships";""",
            cancellationToken
        );
    }
}
