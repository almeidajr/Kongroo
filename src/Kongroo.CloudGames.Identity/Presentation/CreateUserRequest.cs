using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kongroo.CloudGames.Identity.Presentation;

public record CreateUserRequest(
    [property: Required]
    [property: MinLength(4)]
    [property: MaxLength(32)]
    [property: Description("Unique sign-in name for the new user.")]
        string Username,
    [property: Required]
    [property: EmailAddress]
    [property: MaxLength(256)]
    [property: Description("Email address used for account communication and sign-in.")]
        string Email,
    [property: Required]
    [property: MinLength(8)]
    [property: MaxLength(128)]
    [property: DataType(DataType.Password)]
    [property: Description("Plain-text password supplied during account registration.")]
        string Password,
    [property: Required]
    [property: MinLength(2)]
    [property: MaxLength(256)]
    [property: Description("Display name shown for the user profile.")]
        string Name
);
