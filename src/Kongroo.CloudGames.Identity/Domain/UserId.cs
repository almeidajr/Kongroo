namespace Kongroo.CloudGames.Identity.Domain;

public record UserId(Guid Value)
{
    public static UserId Create() => new(Guid.CreateVersion7());
}
