using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.CloudGames.Catalog.Domain;

public record GameCreatedDomainEvent(GameId GameId) : DomainEvent;
