using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Kongroo.CloudGames.Catalog.Presentation;

public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public RouteGroupBuilder MapCatalogEndpoints()
        {
            var routeGroup = endpoints.MapGroup("/catalog").WithTags("Catalog");

            return routeGroup;
        }
    }
}
