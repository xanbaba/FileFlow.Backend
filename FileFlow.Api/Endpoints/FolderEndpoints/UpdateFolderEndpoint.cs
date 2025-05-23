using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class UpdateFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut(Contracts.Endpoints.FolderEndpoints.UpdateFolder, async (
                Guid id,
                RenameFolderRequest request,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                await folderService.RenameAsync(userId, id, request.NewFolderName, cancellationToken);
                var updatedFolder = await folderService.GetMetadataAsync(userId, id, cancellationToken);
                
                return Results.Ok(updatedFolder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status400BadRequest)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(UpdateFolderEndpoint);
}
