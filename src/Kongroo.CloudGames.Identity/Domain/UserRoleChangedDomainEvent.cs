using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Identity.Domain;

public record UserRoleChangedDomainEvent(UserId UserId, UserRole PreviousRole, UserRole CurrentRole) : DomainEvent;
