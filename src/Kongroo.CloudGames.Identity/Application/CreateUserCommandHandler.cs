using System.Security.Cryptography;
using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Kongroo.CloudGames.Identity.Application;

public class CreateUserCommandHandler(IPasswordHasher<string> passwordHasher, IdentityDbContext context)
{
    public async Task<CreateUserResponse> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
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
}
