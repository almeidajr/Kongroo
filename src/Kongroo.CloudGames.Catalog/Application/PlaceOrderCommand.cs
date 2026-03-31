namespace Kongroo.CloudGames.Catalog.Application;

public sealed record PlaceOrderCommand(Guid BuyerId, IReadOnlyList<Guid> GameIds);
