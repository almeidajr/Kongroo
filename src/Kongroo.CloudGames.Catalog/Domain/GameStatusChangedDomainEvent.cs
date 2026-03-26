using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public record GameStatusChangedDomainEvent(GameId GameId, GameStatus PreviousStatus, GameStatus CurrentStatus)
    : DomainEvent;
