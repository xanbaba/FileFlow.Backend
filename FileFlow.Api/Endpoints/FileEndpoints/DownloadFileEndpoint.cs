using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class DownloadFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.DownloadFile,
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    var fileMetadata = await fileService.GetMetadataAsync(userId, id, cancellationToken);
                    var contentStream = await fileService.GetContentAsync(userId, id, cancellationToken);
                    return Results.Stream(contentStream, FileEndpointHelpers.GetMimeType(fileMetadata.Name),
                        fileMetadata.Name, enableRangeProcessing: true);
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK, contentType: "application/octet-stream")
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(DownloadFileEndpoint);
}