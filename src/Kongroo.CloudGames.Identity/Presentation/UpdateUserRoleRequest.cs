using System.ComponentModel;
using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Presentation;

public sealed record UpdateUserRoleRequest(
    [property: Description("Role to assign to the user account.")] UserRole Role
);
