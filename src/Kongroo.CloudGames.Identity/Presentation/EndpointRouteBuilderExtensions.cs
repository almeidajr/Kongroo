using System.ComponentModel;
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
                .MapPost("/users", CreateUserAsync)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("CreateUser")
                .WithSummary("Register a user account")
                .WithDescription(
                    "Creates a user account and returns the public profile information for the new identity."
                );

            routeGroup
                .MapGet("/users/{userId:guid}", GetUserAsync)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("GetUserById")
                .WithSummary("Get a user account")
                .WithDescription("Returns the public profile information for an existing user.");

            return routeGroup;
        }
    }

    private static async Task<Created<CreateUserResponse>> CreateUserAsync(
        CreateUserRequest request,
        CreateUserCommandHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateUserCommand(request.Username, request.Email, request.Password, request.Name);
        var response = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Created($"/users/{response.Id}", response);
    }

    private static async Task<Ok<GetUserResponse>> GetUserAsync(
        [Description("Unique identifier of the user to retrieve.")] Guid userId,
        GetUserQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserQuery(userId);
        var response = await handler.HandleAsync(query, cancellationToken);

        return TypedResults.Ok(response);
    }
}
