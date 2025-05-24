using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class GetChildrenEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FolderEndpoints.GetChildren,
            async (Guid id, IFolderService folderService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var children = await folderService.GetChildrenAsync(userId, id, cancellationToken);
                return Results.Ok(children.Select(child => child.ToResponse()));
            }
        )
            .WithName(Name)
            .RequireAuthorization()
            .Produces<IEnumerable<FileFolderResponse>>()
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(GetChildrenEndpoint);
}