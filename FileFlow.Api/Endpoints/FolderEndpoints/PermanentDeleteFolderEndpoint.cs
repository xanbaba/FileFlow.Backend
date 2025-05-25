using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class PermanentDeleteFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FolderEndpoints.PermanentDeleteFolder, async (
                Guid id,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.DeletePermanentlyAsync(userId, id, cancellationToken);
                
                return Results.NoContent();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Permanently deletes a folder",
                Description = "Permanently removes a folder and all its contents from the system, freeing up the storage space they occupied.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the folder to permanently delete.\n\n" +
                              "### Behavior\n" +
                              "- Only deletes folders that belong to the authenticated user\n" +
                              "- Completely removes the folder, all its subfolders, and all files it contains from the system\n" +
                              "- This action is irreversible - none of the deleted items can be recovered after permanent deletion\n" +
                              "- The storage space will be freed and reflected in the user's quota\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if folder doesn't exist."
            });
    }

    public string Name => nameof(PermanentDeleteFolderEndpoint);
}
