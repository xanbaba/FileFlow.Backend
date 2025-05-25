using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class UpdateFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut(Contracts.Endpoints.FolderEndpoints.UpdateFolder, async (
                Guid id,
                RenameFolderRequest request,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.RenameAsync(userId, id, request.NewFolderName, cancellationToken);
                var updatedFolder = await folderService.GetMetadataAsync(userId, id, cancellationToken);
                
                return Results.Ok(updatedFolder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status400BadRequest)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Renames a folder",
                Description = "Updates the name of a specific folder.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the folder to rename.\n\n" +
                              "### Request Body\n" +
                              "- **NewFolderName** (string): The new name for the folder.\n\n" +
                              "### Behavior\n" +
                              "- Validates that the new name is valid and doesn't contain illegal characters\n" +
                              "- Only renames folders that belong to the authenticated user\n" +
                              "- Cannot rename folders that are in trash\n" +
                              "- Updates folder path and all paths of files and subfolders contained within it\n" +
                              "- If a folder with the same name already exists at the same level, returns a 400 Bad Request error\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or appropriate error status if folder not found or name is invalid."
            });
    }

    public string Name => nameof(UpdateFolderEndpoint);
}
