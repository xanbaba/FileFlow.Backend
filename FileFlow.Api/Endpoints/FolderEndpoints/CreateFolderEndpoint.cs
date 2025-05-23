using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class CreateFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FolderEndpoints.CreateFolder, async (
                CreateFolderRequest request,
                IFolderService folderService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var folder = await folderService.CreateAsync(
                    userId,
                    request.FolderName,
                    request.TargetFolder,
                    cancellationToken);

                return Results.CreatedAtRoute(nameof(GetFolderEndpoint), new {id = folder.Id}, folder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status201Created)
            .Produces<ErrorMessage>(StatusCodes.Status400BadRequest)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(CreateFolderEndpoint);
}
