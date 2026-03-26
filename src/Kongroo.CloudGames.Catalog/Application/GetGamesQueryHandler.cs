using Kongroo.CloudGames.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Application;

public class GetGamesQueryHandler(CatalogDbContext context)
{
    public async Task<IReadOnlyList<GetGameResponse>> HandleAsync(
        GetGamesQuery query,
        CancellationToken cancellationToken
    ) =>
        await context
            .Games.AsNoTracking()
            .OrderBy(game => game.Title)
            .Select(game => new GetGameResponse(
                game.Id.Value,
                game.Title.Value,
                game.Description.Value,
                game.Price.Amount,
                game.Price.Currency.Code,
                game.Status
            ))
            .ToListAsync(cancellationToken);
}
