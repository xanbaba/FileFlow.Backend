using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class MoveItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPatch(Contracts.Endpoints.ItemsEndpoints.MoveItem, async (
                Guid id,
                MoveItemRequest request,
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await itemService.MoveToFolderAsync(userId, id, request.TargetFolderId, cancellationToken);
                return Results.Ok();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status400BadRequest)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Moves an item to a different folder",
                Description = "Relocates a file or folder to a specified destination folder.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file or folder to move.\n\n" +
                              "### Request Body\n" +
                              "- **DestinationFolderId** (string): The ID of the destination folder. Use \"root\" for the root folder or a valid GUID for a specific folder.\n\n" +
                              "### Behavior\n" +
                              "- Only moves items that belong to the authenticated user\n" +
                              "- Updates the ParentId and Path properties of the moved item\n" +
                              "- If moving a folder, also updates paths for all contained files and subfolders\n" +
                              "- Cannot move items that are in trash\n" +
                              "- Cannot move a folder into itself or any of its subfolders (would create a circular reference)\n" +
                              "- If an item with the same name exists in the destination, may return an error or rename the moved item\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or appropriate error status if the operation fails."
            });
    }

    public string Name => nameof(MoveItemEndpoint);
}