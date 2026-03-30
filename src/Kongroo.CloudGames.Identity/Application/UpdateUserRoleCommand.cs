namespace Kongroo.CloudGames.Identity.Application;

public sealed record UpdateUserRoleCommand(Guid ActingUserId, Guid TargetUserId, string Role);
