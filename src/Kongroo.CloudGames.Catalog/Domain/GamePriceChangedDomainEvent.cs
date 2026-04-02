using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.CloudGames.Catalog.Domain;

public record GamePriceChangedDomainEvent(GameId GameId, Money PreviousPrice, Money CurrentPrice) : DomainEvent;
