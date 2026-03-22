using Kongroo.CloudGames.Identity.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Kongroo.CloudGames.Identity.Presentation;

public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public RouteGroupBuilder MapIdentityEndpoints()
        {
            var routeGroup = endpoints.MapGroup("/identity").WithTags("Identity");

            routeGroup
                .MapPost("/users", CreateUser)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("CreateUser")
                .WithSummary("Register a user account")
                .WithDescription(
                    "Creates a user account and returns the public profile information for the new identity."
                );

            return routeGroup;
        }
    }

    private static async Task<Created<CreateUserResponse>> CreateUser(
        CreateUserRequest request,
        CreateUserCommandHandler handler,
        CancellationToken cancellationToken
    )
    {
        var response = await handler.Handle(request, cancellationToken);

        return TypedResults.Created($"/users/{response.Id}", response);
    }
}
