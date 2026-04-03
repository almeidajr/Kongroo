using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.CloudGames.Catalog.Domain;

public sealed record OrderPlacedDomainEvent(
    OrderId OrderId,
    BuyerId BuyerId,
    DateTimeOffset PurchasedAt,
    Money Total,
    IReadOnlyList<GameId> GameIds
) : DomainEvent;
