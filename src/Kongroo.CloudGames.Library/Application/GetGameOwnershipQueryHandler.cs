using Kongroo.CloudGames.Library.Domain;
using Kongroo.CloudGames.Library.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Library.Application;

public class GetGameOwnershipQueryHandler(LibraryDbContext context)
{
    public async Task<GetGameOwnershipResponse> HandleAsync(
        GetGameOwnershipQuery query,
        CancellationToken cancellationToken
    ) =>
        await context
            .GameOwnerships.AsNoTracking()
            .Where(ownership =>
                ownership.Id == GameOwnershipId.From(query.OwnershipId)
                && ownership.OwnerId == OwnerId.From(query.OwnerId)
            )
            .Select(ownership => new GetGameOwnershipResponse(
                ownership.Id.Value,
                ownership.OwnerId.Value,
                ownership.GameId.Value,
                ownership.OrderId.Value,
                ownership.AcquiredAt
            ))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new NotFoundException(nameof(GameOwnership), $"identifier '{query.OwnershipId}'");
}
