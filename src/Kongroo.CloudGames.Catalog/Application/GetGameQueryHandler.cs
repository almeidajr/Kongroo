using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Application;

public class GetGameQueryHandler(CatalogDbContext context, TimeProvider timeProvider)
{
    public async Task<GetGameResponse> HandleAsync(GetGameQuery query, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var game =
            await context
                .Games.AsNoTracking()
                .Where(game => game.Id == GameId.From(query.GameId))
                .Select(game => new GetGameResponse(
                    game.Id.Value,
                    game.Title.Value,
                    game.Description.Value,
                    game.Price.Amount,
                    game.Price.Currency,
                    game.Status,
                    game.Promotions.Where(promotion =>
                            promotion.ActiveRange.Start <= now && now < promotion.ActiveRange.End
                        )
                        .Select(promotion => new GetPromotionResponse(
                            promotion.Id.Value,
                            game.Id.Value,
                            promotion.Discount.Value,
                            promotion.ActiveRange.Start,
                            promotion.ActiveRange.End
                        ))
                        .SingleOrDefault()
                ))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Game), $"identifier '{query.GameId}'");

        return game;
    }
}
