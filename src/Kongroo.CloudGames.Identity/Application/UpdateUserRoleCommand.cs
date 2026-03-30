using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Application;

public sealed record UpdateUserRoleCommand(Guid ActingUserId, Guid TargetUserId, UserRole Role);
