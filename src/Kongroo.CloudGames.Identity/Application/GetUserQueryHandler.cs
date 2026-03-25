using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Application;

public class GetUserQueryHandler(IdentityDbContext context)
{
    public async Task<GetUserResponse> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user =
            await context
                .Users.AsNoTracking()
                .Where(user => user.Id == UserId.From(query.UserId))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"identifier '{query.UserId}'");

        return new GetUserResponse(user.Id.Value, user.Username.Value, user.Email.Value, user.Name.Value);
    }
}
