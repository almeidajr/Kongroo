using Kongroo.CloudGames.Identity.Application;
using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.CloudGames.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shouldly;

namespace Kongroo.CloudGames.IntegrationTests.Identity.Infrastructure;

public sealed class BootstrapAdminServiceTests(PostgreSqlFixture postgreSqlFixture)
    : IClassFixture<PostgreSqlFixture>,
        IAsyncLifetime
{
    private const string Password = "Sup3rSecure!Password";
    private readonly IdentityTestDatabase _database = new(postgreSqlFixture);
    private readonly PasswordHasher<string> _passwordHasher = new();

    [Fact]
    public async Task BootstrapAsync_WithConfiguredBootstrapAdminAndNoUsers_ShouldCreateAdminUser()
    {
        // Arrange
        await using var context = _database.CreateDbContext();
        var service = CreateService(
            context,
            new BootstrapAdminOptions
            {
                Username = "bootstrap-admin",
                Email = "bootstrap-admin@example.com",
                Password = Password,
                Name = "Bootstrap Admin",
            }
        );

        // Act
        await service.BootstrapAsync(TestContext.Current.CancellationToken);

        // Assert
        context.ChangeTracker.Clear();
        var savedUser = await context.Users.SingleAsync(TestContext.Current.CancellationToken);

        savedUser.ShouldSatisfyAllConditions(
            () => savedUser.Username.Value.ShouldBe("bootstrap-admin"),
            () => savedUser.Email.Value.ShouldBe("bootstrap-admin@example.com"),
            () => savedUser.Name.Value.ShouldBe("Bootstrap Admin"),
            () => savedUser.Role.ShouldBe(UserRole.Admin),
            () => savedUser.PasswordHash.Value.ShouldNotBe(Password),
            () =>
                _passwordHasher
                    .VerifyHashedPassword(savedUser.Username.Value, savedUser.PasswordHash.Value, Password)
                    .ShouldBe(PasswordVerificationResult.Success)
        );
    }

    [Fact]
    public async Task BootstrapAsync_WhenAUserAlreadyExists_ShouldDoNothing()
    {
        // Arrange
        await using var context = _database.CreateDbContext();
        var createUserHandler = new CreateUserCommandHandler(_passwordHasher, context);
        await createUserHandler.HandleAsync(
            new CreateUserCommand(
                "existing-user",
                "existing-user@example.com",
                Password,
                "Existing User",
                UserRole.User
            ),
            TestContext.Current.CancellationToken
        );

        var service = CreateService(
            context,
            new BootstrapAdminOptions
            {
                Username = "bootstrap-admin",
                Email = "bootstrap-admin@example.com",
                Password = Password,
                Name = "Bootstrap Admin",
            }
        );

        // Act
        await service.BootstrapAsync(TestContext.Current.CancellationToken);

        // Assert
        context.ChangeTracker.Clear();
        var users = await context
            .Users.OrderBy(user => user.Username)
            .ToListAsync(TestContext.Current.CancellationToken);

        users.Count.ShouldBe(1);
        users
            .Single()
            .ShouldSatisfyAllConditions(
                () => users.Single().Username.Value.ShouldBe("existing-user"),
                () => users.Single().Role.ShouldBe(UserRole.User)
            );
    }

    public async ValueTask InitializeAsync() => await _database.ResetAsync(TestContext.Current.CancellationToken);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private BootstrapAdminService CreateService(IdentityDbContext context, BootstrapAdminOptions options) =>
        new(
            NullLogger<BootstrapAdminService>.Instance,
            Options.Create(options),
            context,
            new CreateUserCommandHandler(_passwordHasher, context)
        );
}
