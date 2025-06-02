using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class PreviewFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.PreviewFile, async (Guid id, IFileService fileService,
                ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var fileMetadata = await fileService.GetMetadataAsync(userId, id, cancellationToken);
                var contentStream = await fileService.GetContentAsync(userId, id, cancellationToken);
                return Results.Stream(contentStream, FileEndpointHelpers.GetMimeType(fileMetadata.Name), enableRangeProcessing: true);
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileStreamResult>(contentType: "application/octet-stream")
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Generates a preview of a file",
                Description = "Provides a preview of the file content, optimized for in-browser viewing.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (Guid): The unique identifier of the file to preview.\n\n" +
                              "### Behavior\n" +
                              "- For images: Returns a possibly resized or compressed version for faster viewing\n" +
                              "- For documents: May return a rendered preview or first few pages\n" +
                              "- For other file types: May return a representation or thumbnail\n" +
                              "- Only allows previewing files that belong to the authenticated user\n" +
                              "- Can preview files even if they are in trash\n\n" +
                              "### Response\n" +
                              "Returns the preview content with appropriate content type headers for browser rendering."
            });
    }

    public string Name => nameof(PreviewFileEndpoint);
}