using System.Security.Claims;
using Kongroo.BuildingBlocks.Domain;

namespace Kongroo.BuildingBlocks.Presentation;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid GetUserId()
        {
            var subject = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(subject, out var userId)
                ? userId
                : throw new UnauthorizedException(nameof(ClaimsPrincipal), "missing or invalid subject claim");
        }
    }
}
