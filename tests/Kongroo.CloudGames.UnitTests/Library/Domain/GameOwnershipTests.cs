using Kongroo.CloudGames.Library.Domain;
using Shouldly;

namespace Kongroo.CloudGames.UnitTests.Library.Domain;

public class GameOwnershipTests
{
    [Fact]
    public void AcquireFromOrder_ShouldCreateGameOwnership()
    {
        // Arrange
        var ownerId = OwnerId.From(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var gameId = GameId.From(Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var orderId = OrderId.From(Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var acquiredAt = new DateTimeOffset(2026, 3, 31, 12, 0, 0, TimeSpan.Zero);

        // Act
        var ownership = GameOwnership.AcquireFromOrder(ownerId, gameId, orderId, acquiredAt);

        // Assert
        ownership.Id.ShouldNotBeNull();
    }

    [Fact]
    public void AcquireFromOrder_ShouldStoreOwnerIdGameIdOrderIdAndAcquiredAt()
    {
        // Arrange
        var ownerId = OwnerId.From(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var gameId = GameId.From(Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var orderId = OrderId.From(Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var acquiredAt = new DateTimeOffset(2026, 3, 31, 12, 0, 0, TimeSpan.Zero);

        // Act
        var ownership = GameOwnership.AcquireFromOrder(ownerId, gameId, orderId, acquiredAt);

        // Assert
        ownership.ShouldSatisfyAllConditions(
            () => ownership.OwnerId.ShouldBe(ownerId),
            () => ownership.GameId.ShouldBe(gameId),
            () => ownership.OrderId.ShouldBe(orderId),
            () => ownership.AcquiredAt.ShouldBe(acquiredAt)
        );
    }

    [Fact]
    public void AcquireFromOrder_ShouldRaiseGameAcquiredDomainEvent()
    {
        // Arrange
        var ownerId = OwnerId.From(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var gameId = GameId.From(Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var orderId = OrderId.From(Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var acquiredAt = new DateTimeOffset(2026, 3, 31, 12, 0, 0, TimeSpan.Zero);

        // Act
        var ownership = GameOwnership.AcquireFromOrder(ownerId, gameId, orderId, acquiredAt);

        // Assert
        var domainEvent = ownership.DomainEvents.Single().ShouldBeOfType<GameAcquiredDomainEvent>();
        domainEvent.ShouldSatisfyAllConditions(
            () => domainEvent.GameOwnershipId.ShouldBe(ownership.Id),
            () => domainEvent.OwnerId.ShouldBe(ownerId),
            () => domainEvent.GameId.ShouldBe(gameId),
            () => domainEvent.OrderId.ShouldBe(orderId),
            () => domainEvent.AcquiredAt.ShouldBe(acquiredAt)
        );
    }
}
