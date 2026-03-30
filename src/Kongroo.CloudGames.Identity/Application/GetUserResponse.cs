using System.ComponentModel;
using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Application;

public sealed record GetUserResponse(
    [property: Description("Unique identifier assigned to the user.")] Guid Id,
    [property: Description("Unique sign-in name chosen by the user.")] string Username,
    [property: Description("Email address associated with the user account.")] string Email,
    [property: Description("Display name shown for the user profile.")] string Name,
    [property: Description("Role currently assigned to the user account.")] UserRole Role
);
