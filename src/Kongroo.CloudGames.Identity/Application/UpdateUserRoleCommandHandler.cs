using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Application;

public class UpdateUserRoleCommandHandler(IdentityDbContext context)
{
    public async Task<GetUserResponse> HandleAsync(UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user =
            await context
                .Users.Where(candidate => candidate.Id == UserId.From(command.TargetUserId))
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"identifier '{command.TargetUserId}'");

        var role = UserRole.From(command.Role);
        ThrowIfSelfDemotion(command.ActingUserId, user.Id.Value, role);

        if (role == UserRole.Admin)
        {
            user.GrantAdmin();
        }
        else
        {
            user.RevokeAdmin();
        }

        await context.SaveChangesAsync(cancellationToken);

        return new GetUserResponse(
            user.Id.Value,
            user.Username.Value,
            user.Email.Value,
            user.Name.Value,
            user.Role.Value
        );
    }

    private static void ThrowIfSelfDemotion(Guid actingUserId, Guid targetUserId, UserRole role)
    {
        if (actingUserId == targetUserId && role == UserRole.User)
        {
            throw new ConflictException(nameof(User), "admins cannot remove their own admin access");
        }
    }
}
