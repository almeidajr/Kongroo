using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public record GameCreatedDomainEvent(GameId GameId) : DomainEvent;
