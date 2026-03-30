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
            .Where(user => user.Id == UserId.From(query.UserId))
            .Select(user => new GetUserResponse(
                user.Id.Value,
                user.Username.Value,
                user.Email.Value,
                user.Name.Value,
                user.Role.Value
            ))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new NotFoundException(nameof(User), $"identifier '{query.UserId}'");
}
