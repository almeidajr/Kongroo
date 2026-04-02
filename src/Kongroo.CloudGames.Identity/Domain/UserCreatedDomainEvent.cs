using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.CloudGames.Identity.Domain;

public record UserCreatedDomainEvent(UserId UserId) : DomainEvent;
