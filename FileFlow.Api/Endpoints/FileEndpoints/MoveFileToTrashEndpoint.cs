using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class MoveFileToTrashEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FileEndpoints.MoveFileToTrash,
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    await fileService.MoveToTrashAsync(userId, id, cancellationToken);
                    return Results.NoContent();
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Moves a file to trash",
                Description = "Marks a file as being in the trash, making it inaccessible from normal views but not deleting it permanently.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to move to trash.\n\n" +
                              "### Behavior\n" +
                              "- Only moves files that belong to the authenticated user\n" +
                              "- Sets the IsInTrash flag to true in the file metadata\n" +
                              "- File remains in the system and can be restored or permanently deleted later\n" +
                              "- Files in trash still count against the user's storage quota\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if file doesn't exist."
            });
    }

    public string Name => nameof(MoveFileToTrashEndpoint);
}