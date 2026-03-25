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
        var username = Username.From(command.Username);
        var email = Email.From(command.Email);
        var name = PersonName.From(command.Name);

        await ThrowIfDuplicateAsync(username, email, cancellationToken);

        var passwordHash = PasswordHash.From(passwordHasher.HashPassword(username.Value, command.Password));
        var user = User.Create(username, email, passwordHash, name);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(user.Id.Value, user.Username.Value, user.Email.Value, user.Name.Value);
    }

    private async Task ThrowIfDuplicateAsync(Username username, Email email, CancellationToken cancellationToken)
    {
        var hasDuplicate = await context
            .Users.AsNoTracking()
            .Where(user => user.Username == username || user.Email == email)
            .AnyAsync(cancellationToken);

        if (hasDuplicate)
        {
            throw new ConflictException(nameof(User), "username or email address is already in use");
        }
    }
}
