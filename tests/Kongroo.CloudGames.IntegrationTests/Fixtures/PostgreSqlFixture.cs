using System.Data.Common;
using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit.Sdk;

namespace Kongroo.CloudGames.IntegrationTests.Fixtures;

public sealed class PostgreSqlFixture(IMessageSink messageSink)
    : DbContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>(messageSink)
{
    public override DbProviderFactory DbProviderFactory => NpgsqlFactory.Instance;

    protected override PostgreSqlBuilder Configure() => new("postgres:18.3");

    public async Task ResetIdentityDatabaseAsync(CancellationToken cancellationToken)
    {
        await using var context = CreateIdentityDbContext();
        await context.Database.MigrateAsync(cancellationToken);
        await context.Database.ExecuteSqlRawAsync(
            $"""TRUNCATE TABLE "{IdentityDbContext.Schema}"."users";""",
            cancellationToken
        );
    }

    public IdentityDbContext CreateIdentityDbContext() =>
        new(
            new DbContextOptionsBuilder<IdentityDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    ConnectionString,
                    postgresOptions => postgresOptions.MigrationsHistoryTable("migrations", IdentityDbContext.Schema)
                )
                .UseSnakeCaseNamingConvention()
                .Options
        );
}
