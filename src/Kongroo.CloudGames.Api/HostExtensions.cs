using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Api;

public static class HostExtensions
{
    extension(IHost host)
    {
        public async Task ApplyMigrationsAsync(CancellationToken cancellationToken = default)
        {
            await using var scope = host.Services.CreateAsyncScope();

            await ApplyMigrationsAsync<IdentityDbContext>(scope.ServiceProvider, cancellationToken);
        }
    }

    private static async Task ApplyMigrationsAsync<TDbContext>(
        IServiceProvider services,
        CancellationToken cancellationToken
    )
        where TDbContext : DbContext
    {
        var context = services.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }
}
