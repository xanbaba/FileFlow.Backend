using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class GetFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.GetFile,
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    var fileMetadata = await fileService.GetMetadataAsync(userId, id, cancellationToken);
                    return Results.Ok(fileMetadata);
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>()
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves metadata for a specific file",
                Description = "Retrieves detailed metadata for a specified file without its content.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to retrieve metadata for.\n\n" +
                              "### Behavior\n" +
                              "- Returns metadata for the specified file if it belongs to the authenticated user\n" +
                              "- Will return file metadata even if the file is in trash\n" +
                              "- Does not return the actual file content (use the download endpoint for that)\n\n" +
                              "### Response\n" +
                              "Returns a FileFolderResponse object containing metadata about the file, including its name, path, size, and other properties."
            });
    }

    public string Name => nameof(GetFileEndpoint);
}