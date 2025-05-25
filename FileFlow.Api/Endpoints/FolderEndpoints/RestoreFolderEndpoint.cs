using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class RestoreFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPatch(Contracts.Endpoints.FolderEndpoints.RestoreFolder, async (
                Guid id,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.RestoreFromTrashAsync(userId, id, cancellationToken);
                var restoredFolder = await folderService.GetMetadataAsync(userId, id, cancellationToken);
                
                return Results.Ok(restoredFolder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Restores a folder from trash",
                Description = "Recovers a folder and all its contents from the trash, making them accessible again in normal views.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the folder to restore from trash.\n\n" +
                              "### Behavior\n" +
                              "- Only restores folders that belong to the authenticated user\n" +
                              "- Sets the IsInTrash flag to false for the folder and all files and subfolders contained within it\n" +
                              "- If the folder's original parent folder no longer exists or is in trash, the folder will be moved to root\n" +
                              "- If a folder with the same name exists in the target location, the restored folder may be renamed\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if folder doesn't exist."
            });
    }

    public string Name => nameof(RestoreFolderEndpoint);
}
