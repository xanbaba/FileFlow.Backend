using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class RenameFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut(Contracts.Endpoints.FileEndpoints.RenameFile,
                async (Guid id, RenameFileRequest request, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    await fileService.RenameAsync(userId, id, request.NewName, cancellationToken);
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(op => new(op)
            {
                Summary = "Renames a file",
                Description = "Updates the name of a specific file while preserving its content and other properties.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to rename.\n\n" +
                              "### Request Body\n" +
                              "- **NewName** (string): The new name for the file, including extension.\n\n" +
                              "### Behavior\n" +
                              "- Only renames files that belong to the authenticated user\n" +
                              "- Validates that the new name is valid (proper characters, length, etc.)\n" +
                              "- Ensures the extension is maintained or is valid for the file type\n" +
                              "- Updates the file's name and path properties\n" +
                              "- Cannot rename files that are in trash\n" +
                              "- If a file with the same name already exists in the same folder, returns a 400 Bad Request error\n\n" +
                              "### Response\n" +
                              "Returns 204 No Content if successful.\n" +
                              "Returns 404 Not Found if the file doesn't exist.\n" +
                              "Returns 400 Bad Request if the new filename is invalid or already exists in the same location."
            });
    }

    public string Name => nameof(RenameFileEndpoint);
}