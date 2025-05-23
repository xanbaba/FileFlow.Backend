using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class RestoreFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPatch(Contracts.Endpoints.FolderEndpoints.RestoreFolder, async (
                Guid id,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.RestoreFromTrashAsync(userId, id, cancellationToken);
                var restoredFolder = await folderService.GetMetadataAsync(userId, id, cancellationToken);
                
                return Results.Ok(restoredFolder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(RestoreFolderEndpoint);
}
