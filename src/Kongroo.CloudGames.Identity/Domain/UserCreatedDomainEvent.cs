using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Identity.Domain;

public record UserCreatedDomainEvent(UserId UserId) : DomainEvent;
