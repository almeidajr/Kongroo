using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.CloudGames.Catalog.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Catalog.Application;

public class DeleteGameCommandHandler(CatalogDbContext context)
{
    public async Task HandleAsync(DeleteGameCommand command, CancellationToken cancellationToken)
    {
        var game =
            await context
                .Games.Where(game => game.Id == GameId.From(command.GameId))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Game), $"identifier '{command.GameId}'");

        context.Games.Remove(game);
        await context.SaveChangesAsync(cancellationToken);
    }
}
