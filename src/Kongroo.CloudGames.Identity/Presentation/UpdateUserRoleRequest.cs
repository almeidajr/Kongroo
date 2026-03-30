using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Presentation;

public sealed record UpdateUserRoleRequest(
    [property: Required]
    [property: MaxLength(UserRole.MaxLength)]
    [property: RegularExpression("^(user|admin)$", ErrorMessage = "Role must be either 'user' or 'admin'.")]
    [property: Description("Role to assign to the user account. Allowed values: user, admin.")]
        string Role
);
