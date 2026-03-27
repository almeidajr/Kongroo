using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Application;

public class GetGameQueryHandler(CatalogDbContext context)
{
    public async Task<GetGameResponse> HandleAsync(GetGameQuery query, CancellationToken cancellationToken)
    {
        var game =
            await context
                .Games.AsNoTracking()
                .Where(game => game.Id == GameId.From(query.GameId))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Game), $"identifier '{query.GameId}'");

        return new GetGameResponse(
            game.Id.Value,
            game.Title.Value,
            game.Description.Value,
            game.Price.Amount,
            game.Price.Currency,
            game.Status
        );
    }
}
