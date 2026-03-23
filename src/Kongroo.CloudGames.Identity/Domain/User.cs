using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Identity.Domain;

public class User : Entity<UserId>
{
    public const int UsernameMinLength = 4;
    public const int UsernameMaxLength = 32;
    public const int EmailMaxLength = 256;
    public const int PasswordHashMaxLength = 256;
    public const int SecurityStampLength = 32;
    public const int NameMinLength = 2;
    public const int NameMaxLength = 256;

    private User() { }

    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string PasswordHash { get; init; }

    public required string SecurityStamp { get; init; }

    public required string Name { get; init; }

    public required UserRole Role { get; set; }

    public static User Create(string username, string email, string passwordHash, string securityStamp, string name)
    {
        var user = new User
        {
            Id = UserId.Create(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            SecurityStamp = securityStamp,
            Name = name,
            Role = UserRole.User,
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }

    public void GrantAdmin() => ChangeRole(UserRole.Admin);

    public void RevokeAdmin() => ChangeRole(UserRole.User);

    private void ChangeRole(UserRole role)
    {
        if (Role == role)
        {
            return;
        }

        var previousRole = Role;
        Role = role;
        RaiseDomainEvent(new UserRoleChangedDomainEvent(Id, previousRole, Role));
    }
}
