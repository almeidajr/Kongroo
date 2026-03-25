namespace Kongroo.CloudGames.Identity.Domain;

public record UserRole(string Value)
{
    public const int MaxLength = 16;

    public static readonly UserRole User = new("user");
    public static readonly UserRole Admin = new("admin");

    public static UserRole From(string value) =>
        value switch
        {
            "user" => User,
            "admin" => Admin,
            _ => throw new ArgumentException($"Invalid user role: {value}", nameof(value)),
        };
}
