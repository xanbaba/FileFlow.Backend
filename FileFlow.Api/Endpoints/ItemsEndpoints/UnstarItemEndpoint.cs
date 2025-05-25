using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class UnstarItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.ItemsEndpoints.UnstarItem, async (
                Guid id,
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await itemService.UnstarAsync(userId, id, cancellationToken);
                return Results.Ok();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Removes starred status from an item",
                Description = "Removes the starred marking from a specific file or folder.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file or folder to unstar.\n\n" +
                              "### Behavior\n" +
                              "- Only affects items that belong to the authenticated user\n" +
                              "- Sets the IsStarred flag to false in the item's metadata\n" +
                              "- Works on both files and folders\n" +
                              "- Can unstar items even if they are in trash\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if the item doesn't exist."
            });
    }

    public string Name => nameof(UnstarItemEndpoint);
}