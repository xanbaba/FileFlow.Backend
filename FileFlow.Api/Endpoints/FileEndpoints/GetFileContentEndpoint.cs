using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class GetFileContentEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.GetFileContent, async (Guid id, IFileService fileService,
                ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var content = await fileService.GetContentAsync(userId, id, cancellationToken);
                return Results.Stream(content.stream, FileEndpointHelpers.GetMimeType(content.file.Name),
                    enableRangeProcessing: true);
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileStreamResult>(contentType: "application/octet-stream")
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Gets content of the file",
                Description = "Provides the actual content of a file with appropriate content type and filename.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file.\n\n" +
                              "### Behavior\n" +
                              "- Retrieves file metadata to determine the name and content type\n" +
                              "- Streams the file content with range processing enabled for supporting large files\n" +
                              "- Only allows accessing files that belong to the authenticated user\n\n" +
                              "### Response\n" +
                              "Returns a stream containing the file content with the appropriate MIME type and original filename."
            });
    }

    public string Name => nameof(GetFileContentEndpoint);
}