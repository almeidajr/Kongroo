using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Application;

public class UpdateGameCommandHandler(CatalogDbContext context)
{
    public async Task<GetGameResponse> HandleAsync(UpdateGameCommand command, CancellationToken cancellationToken)
    {
        var game =
            await context
                .Games.Where(game => game.Id == GameId.From(command.GameId))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Game), $"identifier '{command.GameId}'");

        game.ChangeDetails(GameTitle.From(command.Title), GameDescription.From(command.Description));
        game.ChangePrice(Money.From(command.PriceAmount, command.Currency));
        game.ChangeStatus(command.Status);

        await context.SaveChangesAsync(cancellationToken);

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
