using Kongroo.CloudGames.Identity.Application;
using Kongroo.CloudGames.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kongroo.CloudGames.Identity.Infrastructure;

public sealed class BootstrapAdminService(
    ILogger<BootstrapAdminService> logger,
    IOptions<BootstrapAdminOptions> options,
    IdentityDbContext context,
    CreateUserCommandHandler handler
)
{
    private readonly BootstrapAdminOptions _options = options.Value;

    public async Task BootstrapAsync(CancellationToken cancellationToken)
    {
        if (await context.Users.AsNoTracking().AnyAsync(cancellationToken))
        {
            logger.LogInformation("Bootstrap admin skipped because at least one user already exists.");
            return;
        }

        var response = await handler.HandleAsync(
            new CreateUserCommand(_options.Username, _options.Email, _options.Password, _options.Name, UserRole.Admin),
            cancellationToken
        );

        logger.LogInformation("Bootstrap admin user {Username} was created.", response.Username);
    }
}
