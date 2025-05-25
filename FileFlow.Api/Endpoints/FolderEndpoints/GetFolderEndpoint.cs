using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class GetFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FolderEndpoints.GetFolder, async (
                Guid id,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var folder = await folderService.GetMetadataAsync(userId, id, cancellationToken);
                return Results.Ok(folder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>()
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves metadata for a specific folder",
                Description = "Retrieves detailed metadata for a specified folder.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the folder to retrieve.\n\n" +
                              "### Behavior\n" +
                              "- Returns metadata for the specified folder if it belongs to the authenticated user\n" +
                              "- Will return folder metadata even if the folder is in trash\n\n" +
                              "### Response\n" +
                              "Returns a FileFolderResponse object containing metadata about the folder, including its name, path, and other properties."
            });
    }

    public string Name => nameof(GetFolderEndpoint);
}
