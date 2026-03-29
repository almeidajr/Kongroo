using Kongroo.CloudGames.Catalog.Application;
using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Kongroo.SharedKernel.Exceptions;
using Shouldly;

namespace Kongroo.CloudGames.IntegrationTests.Catalog.Application;

public sealed class GetGameQueryHandlerTests(PostgreSqlFixture postgreSqlFixture)
    : IClassFixture<PostgreSqlFixture>,
        IAsyncLifetime
{
    private readonly CatalogTestDatabase _database = new(postgreSqlFixture);

    [Fact]
    public async Task HandleAsync_WithExistingGameId_ShouldReturnGame()
    {
        // Arrange
        await using var context = _database.CreateDbContext();
        var gameId = await CreateGameAsync(context, TestContext.Current.CancellationToken);

        var handler = new GetGameQueryHandler(context);

        // Act
        var response = await handler.HandleAsync(new GetGameQuery(gameId.Value), TestContext.Current.CancellationToken);

        // Assert
        response.ShouldSatisfyAllConditions(
            () => response.Id.ShouldBe(gameId.Value),
            () => response.Title.ShouldBe("Portal"),
            () => response.Description.ShouldBe("A puzzle platformer."),
            () => response.PriceAmount.ShouldBe(19.99m),
            () => response.Currency.ShouldBe(Currency.Usd),
            () => response.Status.ShouldBe(GameStatus.Draft)
        );
    }

    [Fact]
    public async Task HandleAsync_WhenGameDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        await using var context = _database.CreateDbContext();
        var missingGameId = Guid.NewGuid();
        var handler = new GetGameQueryHandler(context);

        // Act
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetGameQuery(missingGameId), TestContext.Current.CancellationToken)
        );

        // Assert
        exception.ResourceName.ShouldBe(nameof(Game));
        exception.Lookup.ShouldBe($"identifier '{missingGameId}'");
    }

    public async ValueTask InitializeAsync() => await _database.ResetAsync(TestContext.Current.CancellationToken);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private static async Task<GameId> CreateGameAsync(CatalogDbContext context, CancellationToken cancellationToken)
    {
        var handler = new CreateGameCommandHandler(context);
        var response = await handler.HandleAsync(
            new CreateGameCommand("Portal", "A puzzle platformer.", 19.99m, Currency.Usd),
            cancellationToken
        );
        return new GameId(response.Id);
    }
}
