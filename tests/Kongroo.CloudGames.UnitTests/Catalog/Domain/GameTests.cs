using Kongroo.CloudGames.Catalog.Domain;
using Shouldly;

namespace Kongroo.CloudGames.UnitTests.Catalog.Domain;

public class GameTests
{
    [Fact]
    public void Create_WithValidValues_ShouldInitializeGameWithDraftStatus()
    {
        // Arrange
        var title = GameTitle.From("Portal");
        var description = GameDescription.From("A puzzle platformer.");
        var price = Money.From(19.99m, Currency.Usd);

        // Act
        var game = Game.Create(title, description, price);

        // Assert
        game.Status.ShouldBe(GameStatus.Draft);
    }

    [Fact]
    public void Create_WithValidValues_ShouldRaiseCreatedEvent()
    {
        // Arrange
        var title = GameTitle.From("Portal");
        var description = GameDescription.From("A puzzle platformer.");
        var price = Money.From(19.99m, Currency.Usd);

        // Act
        var game = Game.Create(title, description, price);

        // Assert
        var domainEvent = game.DomainEvents.Single().ShouldBeOfType<GameCreatedDomainEvent>();
        domainEvent.GameId.ShouldBe(game.Id);
    }

    [Fact]
    public void ChangeDetails_WithValidValues_ShouldUpdateTitleAndDescription()
    {
        // Arrange
        var game = CreateGame();
        var updatedTitle = GameTitle.From("Portal 2");
        var updatedDescription = GameDescription.From("A cooperative puzzle platformer.");

        // Act
        game.ChangeDetails(updatedTitle, updatedDescription);

        // Assert
        game.Title.ShouldBe(updatedTitle);
        game.Description.ShouldBe(updatedDescription);
    }

    [Fact]
    public void ChangeDetails_WithValidValues_ShouldRaiseDetailsChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        var previousTitle = game.Title;
        var previousDescription = game.Description;
        var updatedTitle = GameTitle.From("Portal 2");
        var updatedDescription = GameDescription.From("A cooperative puzzle platformer.");
        game.ClearDomainEvents();

        // Act
        game.ChangeDetails(updatedTitle, updatedDescription);

        // Assert
        var domainEvent = game.DomainEvents.Single().ShouldBeOfType<GameDetailsChangedDomainEvent>();
        domainEvent.GameId.ShouldBe(game.Id);
        domainEvent.PreviousTitle.ShouldBe(previousTitle);
        domainEvent.CurrentTitle.ShouldBe(updatedTitle);
        domainEvent.PreviousDescription.ShouldBe(previousDescription);
        domainEvent.CurrentDescription.ShouldBe(updatedDescription);
    }

    [Fact]
    public void ChangeDetails_WithSameValues_ShouldNotRaiseDetailsChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        game.ClearDomainEvents();

        // Act
        game.ChangeDetails(game.Title, game.Description);

        // Assert
        game.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void ChangePrice_WithValidValue_ShouldUpdatePrice()
    {
        // Arrange
        var game = CreateGame();
        var updatedPrice = Money.From(29.99m, Currency.Eur);

        // Act
        game.ChangePrice(updatedPrice);

        // Assert
        game.Price.ShouldBe(updatedPrice);
    }

    [Fact]
    public void ChangePrice_WithValidValue_ShouldRaisePriceChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        var previousPrice = game.Price;
        var updatedPrice = Money.From(29.99m, Currency.Eur);
        game.ClearDomainEvents();

        // Act
        game.ChangePrice(updatedPrice);

        // Assert
        var domainEvent = game.DomainEvents.Single().ShouldBeOfType<GamePriceChangedDomainEvent>();
        domainEvent.GameId.ShouldBe(game.Id);
        domainEvent.PreviousPrice.ShouldBe(previousPrice);
        domainEvent.CurrentPrice.ShouldBe(updatedPrice);
    }

    [Fact]
    public void ChangePrice_WithSameValue_ShouldNotRaisePriceChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        game.ClearDomainEvents();

        // Act
        game.ChangePrice(game.Price);

        // Assert
        game.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void ChangeStatus_WithDefinedStatus_ShouldRaiseStatusChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        game.ClearDomainEvents();

        // Act
        game.ChangeStatus(GameStatus.Published);

        // Assert
        var domainEvent = game.DomainEvents.Single().ShouldBeOfType<GameStatusChangedDomainEvent>();
        domainEvent.GameId.ShouldBe(game.Id);
        domainEvent.PreviousStatus.ShouldBe(GameStatus.Draft);
        domainEvent.CurrentStatus.ShouldBe(GameStatus.Published);
    }

    [Fact]
    public void ChangeStatus_WithSameStatus_ShouldNotRaiseStatusChangedEvent()
    {
        // Arrange
        var game = CreateGame();
        game.ClearDomainEvents();

        // Act
        game.ChangeStatus(game.Status);

        // Assert
        game.DomainEvents.ShouldBeEmpty();
    }

    private static Game CreateGame() =>
        Game.Create(
            GameTitle.From("Portal"),
            GameDescription.From("A puzzle platformer."),
            Money.From(19.99m, Currency.Usd)
        );
}
