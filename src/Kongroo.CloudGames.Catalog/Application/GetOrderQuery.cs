namespace Kongroo.CloudGames.Catalog.Application;

public sealed record GetOrderQuery(Guid BuyerId, Guid OrderId);
