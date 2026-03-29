using Kongroo.CloudGames.Catalog.Application;
using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Shouldly;

namespace Kongroo.CloudGames.IntegrationTests.Catalog.Application;

public sealed class GetGamesQueryHandlerTests(PostgreSqlFixture postgreSqlFixture)
    : IClassFixture<PostgreSqlFixture>,
        IAsyncLifetime
{
    private readonly CatalogTestDatabase _database = new(postgreSqlFixture);

    [Fact]
    public async Task HandleAsync_WithNoGames_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = _database.CreateDbContext();
        var handler = new GetGamesQueryHandler(context);

        // Act
        var response = await handler.HandleAsync(new GetGamesQuery(), TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithExistingGames_ShouldReturnGamesOrderedByTitle()
    {
        // Arrange
        await using var context = _database.CreateDbContext();

        await CreateGameAsync(
            new CreateGameCommand("Zelda", "Adventure game.", 59.99m, Currency.Usd),
            context,
            TestContext.Current.CancellationToken
        );
        await CreateGameAsync(
            new CreateGameCommand("Portal", "Puzzle platformer.", 19.99m, Currency.Eur),
            context,
            TestContext.Current.CancellationToken
        );
        await CreateGameAsync(
            new CreateGameCommand("Celeste", "Precision platformer.", 24.99m, Currency.Brl),
            context,
            TestContext.Current.CancellationToken
        );

        var handler = new GetGamesQueryHandler(context);

        // Act
        var response = await handler.HandleAsync(new GetGamesQuery(), TestContext.Current.CancellationToken);

        // Assert
        response.Select(game => game.Title).ShouldBe(["Celeste", "Portal", "Zelda"]);
    }

    public async ValueTask InitializeAsync() => await _database.ResetAsync(TestContext.Current.CancellationToken);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private static async Task CreateGameAsync(
        CreateGameCommand command,
        CatalogDbContext context,
        CancellationToken cancellationToken
    )
    {
        var handler = new CreateGameCommandHandler(context);
        await handler.HandleAsync(command, cancellationToken);
    }
}
