using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.CloudGames.Catalog.Domain;

public record GameDetailsChangedDomainEvent(
    GameId GameId,
    GameTitle PreviousTitle,
    GameTitle CurrentTitle,
    GameDescription PreviousDescription,
    GameDescription CurrentDescription
) : DomainEvent;
