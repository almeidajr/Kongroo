using Kongroo.CloudGames.Identity.Domain;
using Shouldly;

namespace Kongroo.CloudGames.UnitTests.Identity.Domain;

public class UserTests
{
    [Fact]
    public void Create_WithValidValues_ShouldInitializeUserWithDefaultRole()
    {
        // Arrange
        const string username = "kongroo";
        const string email = "kongroo@example.com";
        const string passwordHash = "password-hash";
        const string securityStamp = "security-stamp";
        const string name = "Kongroo";

        // Act
        var user = User.Create(username, email, passwordHash, securityStamp, name);

        // Assert
        user.Role.ShouldBe(UserRole.User);
    }

    [Fact]
    public void Create_WithValidValues_ShouldRaiseCreatedEvent()
    {
        // Arrange
        const string username = "kongroo";
        const string email = "kongroo@example.com";
        const string passwordHash = "password-hash";
        const string securityStamp = "security-stamp";
        const string name = "Kongroo";

        // Act
        var user = User.Create(username, email, passwordHash, securityStamp, name);

        // Assert
        var domainEvent = user.DomainEvents.Single().ShouldBeOfType<UserCreatedDomainEvent>();
        domainEvent.UserId.ShouldBe(user.Id);
    }

    [Fact]
    public void GrantAdmin_WhenUserRoleIsUser_ShouldPromoteUser()
    {
        // Arrange
        var user = CreateUser();
        user.ClearDomainEvents();

        // Act
        user.GrantAdmin();

        // Assert
        user.Role.ShouldBe(UserRole.Admin);
    }

    [Fact]
    public void GrantAdmin_WhenUserRoleIsUser_ShouldRaiseRoleChangedEvent()
    {
        // Arrange
        var user = CreateUser();
        user.ClearDomainEvents();

        // Act
        user.GrantAdmin();

        // Assert
        var domainEvent = user.DomainEvents.Single().ShouldBeOfType<UserRoleChangedDomainEvent>();
        domainEvent.UserId.ShouldBe(user.Id);
        domainEvent.PreviousRole.ShouldBe(UserRole.User);
        domainEvent.CurrentRole.ShouldBe(UserRole.Admin);
    }

    [Fact]
    public void GrantAdmin_WhenUserIsAlreadyAdmin_ShouldKeepUserAsAdmin()
    {
        // Arrange
        var user = CreateUser();
        user.GrantAdmin();
        user.ClearDomainEvents();

        // Act
        user.GrantAdmin();

        // Assert
        user.Role.ShouldBe(UserRole.Admin);
    }

    [Fact]
    public void GrantAdmin_WhenUserIsAlreadyAdmin_ShouldNotRaiseAnotherRoleChangedEvent()
    {
        // Arrange
        var user = CreateUser();
        user.GrantAdmin();
        user.ClearDomainEvents();

        // Act
        user.GrantAdmin();

        // Assert
        user.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void RevokeAdmin_WhenUserRoleIsAdmin_ShouldDemoteUser()
    {
        // Arrange
        var user = CreateUser();
        user.GrantAdmin();
        user.ClearDomainEvents();

        // Act
        user.RevokeAdmin();

        // Assert
        user.Role.ShouldBe(UserRole.User);
    }

    [Fact]
    public void RevokeAdmin_WhenUserRoleIsAdmin_ShouldRaiseRoleChangedEvent()
    {
        // Arrange
        var user = CreateUser();
        user.GrantAdmin();
        user.ClearDomainEvents();

        // Act
        user.RevokeAdmin();

        // Assert
        user.DomainEvents.Count.ShouldBe(1);
        var domainEvent = user.DomainEvents.Single().ShouldBeOfType<UserRoleChangedDomainEvent>();
        domainEvent.UserId.ShouldBe(user.Id);
        domainEvent.PreviousRole.ShouldBe(UserRole.Admin);
        domainEvent.CurrentRole.ShouldBe(UserRole.User);
    }

    [Fact]
    public void RevokeAdmin_WhenUserIsAlreadyUser_ShouldKeepUserAsUser()
    {
        // Arrange
        var user = CreateUser();
        user.ClearDomainEvents();

        // Act
        user.RevokeAdmin();

        // Assert
        user.Role.ShouldBe(UserRole.User);
    }

    [Fact]
    public void RevokeAdmin_WhenUserIsAlreadyUser_ShouldNotRaiseRoleChangedEvent()
    {
        // Arrange
        var user = CreateUser();
        user.ClearDomainEvents();

        // Act
        user.RevokeAdmin();

        // Assert
        user.DomainEvents.ShouldBeEmpty();
    }

    private static User CreateUser() =>
        User.Create(
            username: "kongroo",
            email: "kongroo@example.com",
            passwordHash: "password-hash",
            securityStamp: "security-stamp",
            name: "Kongroo"
        );
}
