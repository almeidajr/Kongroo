using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public record OrderPlacedDomainEvent(OrderId OrderId, BuyerId BuyerId, Money Total) : DomainEvent;
