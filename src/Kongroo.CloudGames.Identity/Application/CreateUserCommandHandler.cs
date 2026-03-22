using System.Security.Cryptography;
using Kongroo.CloudGames.Identity.Domain;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.CloudGames.Identity.Presentation;
using Microsoft.AspNetCore.Identity;

namespace Kongroo.CloudGames.Identity.Application;

public class CreateUserCommandHandler(IPasswordHasher<string> passwordHasher, IdentityDbContext context)
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.Username,
            request.Email,
            passwordHasher.HashPassword(request.Username, request.Password),
            RandomNumberGenerator.GetHexString(User.SecurityStampLength),
            request.Name
        );

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(user.Id.Value, user.Username, user.Email, user.Name);
    }
}
