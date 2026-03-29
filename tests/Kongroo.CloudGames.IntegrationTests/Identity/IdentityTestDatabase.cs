using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.IntegrationTests.Identity;

public sealed class IdentityTestDatabase(PostgreSqlFixture fixture)
{
    public IdentityDbContext CreateDbContext() =>
        new(
            new DbContextOptionsBuilder<IdentityDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    fixture.ConnectionString,
                    postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", IdentityDbContext.Schema)
                )
                .UseSnakeCaseNamingConvention()
                .Options
        );

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync(cancellationToken);
        await context.Database.ExecuteSqlRawAsync(
            $"""TRUNCATE TABLE "{IdentityDbContext.Schema}"."users";""",
            cancellationToken
        );
    }
}
