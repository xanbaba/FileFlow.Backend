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
                var children = await folderService.GetChildrenAsync(userId, id, cancellationToken);
                
                return Results.Ok(new FolderDetailsResponse
                {
                    Folder = folder.ToResponse(),
                    Children = children.Select(Mapper.ToResponse).ToList()
                });
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FolderDetailsResponse>(StatusCodes.Status200OK)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(GetFolderEndpoint);
}
