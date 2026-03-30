using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Application;

public class GetUsersQueryHandler(IdentityDbContext context)
{
    public async Task<IReadOnlyList<GetUserResponse>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken
    ) =>
        await context
            .Users.AsNoTracking()
            .OrderBy(user => user.Username)
            .Select(user => new GetUserResponse(
                user.Id.Value,
                user.Username.Value,
                user.Email.Value,
                user.Name.Value,
                user.Role
            ))
            .ToListAsync(cancellationToken);
}
