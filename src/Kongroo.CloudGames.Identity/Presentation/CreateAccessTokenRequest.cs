using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Presentation;

public sealed record CreateAccessTokenRequest(
    [property: Required]
    [property: MinLength(User.UsernameMinLength)]
    [property: MaxLength(User.UsernameMaxLength)]
    [property: Description("Unique sign-in name used to authenticate the user.")]
        string Username,
    [property: Required]
    [property: MaxLength(128)]
    [property: DataType(DataType.Password)]
    [property: Description("Plain-text password supplied during sign-in.")]
        string Password
);
