using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kongroo.SharedKernel.Exceptions;

namespace Kongroo.CloudGames.Identity.Presentation;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid GetUserId()
        {
            var subject = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(subject, out var userId)
                ? userId
                : throw new UnauthorizedException(nameof(ClaimsPrincipal), "missing or invalid subject claim");
        }
    }
}
