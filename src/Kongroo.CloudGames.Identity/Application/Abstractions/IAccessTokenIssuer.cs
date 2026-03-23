using Kongroo.CloudGames.Identity.Domain;

namespace Kongroo.CloudGames.Identity.Application.Abstractions;

public interface IAccessTokenIssuer
{
    AuthenticateUserResponse IssueToken(User user);
}
