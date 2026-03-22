using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Application;

public class GetUserQueryHandler(IdentityDbContext context)
{
    public async Task<GetUserResponse> HandleAsync(GetUserQuery query, CancellationToken cancellationToken) =>
        await context
            .Users.AsNoTracking()
            .Where(user => user.Id == new UserId(query.UserId))
            .Select(user => new GetUserResponse(user.Id.Value, user.Username, user.Email, user.Name))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new NotFoundException(nameof(User), $"identifier '{query.UserId}'");
}
