using System.Security.Cryptography;
using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kongroo.CloudGames.Identity.Application;

public class CreateUserCommandHandler(IPasswordHasher<string> passwordHasher, IdentityDbContext context)
{
    public async Task<CreateUserResponse> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await ThrowIfDuplicateAsync(command, cancellationToken);

        var user = User.Create(
            command.Username,
            command.Email,
            passwordHasher.HashPassword(command.Username, command.Password),
            RandomNumberGenerator.GetHexString(User.SecurityStampLength),
            command.Name
        );

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(user.Id.Value, user.Username, user.Email, user.Name);
    }

    private async Task ThrowIfDuplicateAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var hasDuplicate = await context
            .Users.AsNoTracking()
            .Where(user => user.Username == command.Username || user.Email == command.Email)
            .AnyAsync(cancellationToken);

        if (hasDuplicate)
        {
            throw new ConflictException(nameof(User), "username or email address is already in use");
        }
    }
}
