using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;

namespace Kongroo.CloudGames.Catalog.Application;

public class CreateGameCommandHandler(CatalogDbContext context)
{
    public async Task<CreateGameResponse> HandleAsync(CreateGameCommand command, CancellationToken cancellationToken)
    {
        var title = GameTitle.From(command.Title);
        var description = GameDescription.From(command.Description);
        var price = Money.From(command.PriceAmount, Currency.From(command.Currency));
        var game = Game.Create(title, description, price);

        context.Games.Add(game);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateGameResponse(
            game.Id.Value,
            game.Title.Value,
            game.Description.Value,
            game.Price.Amount,
            game.Price.Currency.Code,
            game.Status
        );
    }
}
