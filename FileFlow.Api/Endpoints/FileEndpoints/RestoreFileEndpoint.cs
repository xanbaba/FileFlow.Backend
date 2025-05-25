using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class RestoreFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.RestoreFile, 
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await fileService.RestoreFromTrashAsync(userId, id, cancellationToken);
                return Results.NoContent();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Restores a file from trash",
                Description = "Recovers a file from the trash, making it accessible again in normal views.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to restore from trash.\n\n" +
                              "### Behavior\n" +
                              "- Only restores files that belong to the authenticated user\n" +
                              "- Sets the IsInTrash flag to false in the file metadata\n" +
                              "- If the file's original parent folder no longer exists or is in trash, the file will be moved to root\n" +
                              "- If a file with the same name exists in the target location, the restored file may be renamed\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful, or 404 Not Found if file doesn't exist."
            });
    }

    public string Name => nameof(RestoreFileEndpoint);
}
