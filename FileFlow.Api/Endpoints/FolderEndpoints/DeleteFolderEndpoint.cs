using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class DeleteFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FolderEndpoints.DeleteFolder, async (
                Guid id,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.MoveToTrashAsync(userId, id, cancellationToken);
                
                return Results.NoContent();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Moves a folder to trash",
                Description = "Marks a folder and all its contents as being in the trash, making them inaccessible from normal views but not deleting them permanently.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the folder to move to trash.\n\n" +
                              "### Behavior\n" +
                              "- Only moves folders that belong to the authenticated user\n" +
                              "- Sets the IsInTrash flag to true for the folder and all files and subfolders contained within it\n" +
                              "- Folder and its contents remain in the system and can be restored or permanently deleted later\n" +
                              "- Items in trash still count against the user's storage quota\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if folder doesn't exist."
            });
    }

    public string Name => nameof(DeleteFolderEndpoint);
}
