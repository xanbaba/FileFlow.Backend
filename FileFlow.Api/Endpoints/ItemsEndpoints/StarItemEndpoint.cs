using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class StarItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPatch(Contracts.Endpoints.ItemsEndpoints.StarItem, async (
                Guid id,
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await itemService.StarAsync(userId, id, cancellationToken);
                return Results.Ok();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Marks an item as starred",
                Description = "Marks a specific file or folder as starred for easy access.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file or folder to star.\n\n" +
                              "### Behavior\n" +
                              "- Only stars items that belong to the authenticated user\n" +
                              "- Sets the IsStarred flag to true in the item's metadata\n" +
                              "- Works on both files and folders\n" +
                              "- Can star items that are in trash, though they won't appear in the starred items view until restored\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if the item doesn't exist."
            });
    }

    public string Name => nameof(StarItemEndpoint);
}