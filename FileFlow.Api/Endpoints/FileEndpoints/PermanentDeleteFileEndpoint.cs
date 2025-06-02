using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class PermanentDeleteFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FileEndpoints.PermanentDeleteFile,
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    await fileService.DeletePermanentlyAsync(userId, id, cancellationToken);
                    return Results.Ok();
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Permanently deletes a file",
                Description = "Permanently removes a file from the system, freeing up the storage space it occupied.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to permanently delete.\n\n" +
                              "### Behavior\n" +
                              "- Only deletes files that belong to the authenticated user\n" +
                              "- Completely removes the file content and metadata from the system\n" +
                              "- This action is irreversible - the file cannot be recovered after permanent deletion\n" +
                              "- The storage space will be freed and reflected in the user's quota\n\n" +
                              "### Response\n" +
                              "Returns 200 Ok if successful, or 404 Not Found if file doesn't exist."
            });
    }

    public string Name => nameof(PermanentDeleteFileEndpoint);
}