using Kongroo.CloudGames.Identity.Domain;
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

    private static Created<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var response = new CreateUserResponse(UserId.Create().Value, request.Username, request.Email, request.Name);

        return TypedResults.Created($"/users/{response.Id}", response);
    }
}
