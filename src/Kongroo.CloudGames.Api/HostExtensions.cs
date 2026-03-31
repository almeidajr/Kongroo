using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.CloudGames.Library.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Api;

public static class HostExtensions
{
    extension(IHost host)
    {
        public async Task ApplyMigrationsAsync(CancellationToken cancellationToken = default)
        {
            await using var scope = host.Services.CreateAsyncScope();

            await ApplyMigrationsAsync<CatalogDbContext>(scope.ServiceProvider, cancellationToken);
            await ApplyMigrationsAsync<IdentityDbContext>(scope.ServiceProvider, cancellationToken);
            await ApplyMigrationsAsync<LibraryDbContext>(scope.ServiceProvider, cancellationToken);
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
