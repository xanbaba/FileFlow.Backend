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
                    request.TargetFolderId,
                    cancellationToken);

                return Results.CreatedAtRoute(nameof(GetFolderEndpoint), new {id = folder.Id}, folder.ToResponse());
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>(StatusCodes.Status201Created)
            .Produces<ErrorMessage>(StatusCodes.Status400BadRequest)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Creates a new folder",
                Description = "Creates a new folder in the specified location or at the root level.\n\n" +
                              "### Request Body\n" +
                              "- **FolderName** (string): The name for the new folder\n" +
                              "- **TargetFolderId** (string, optional): The Id of the parent folder where the new folder should be created. If null, creates at root level\n\n" +
                              "### Behavior\n" +
                              "- Validates that the folder name is valid and doesn't contain illegal characters\n" +
                              "- Creates a new folder with the specified name in the target location\n" +
                              "- If a folder with the same name already exists, returns a 400 Bad Request error\n" +
                              "- Folder names are case-sensitive\n\n" +
                              "### Response\n" +
                              "Returns a FileFolderResponse object containing metadata about the newly created folder."
            });
    }

    public string Name => nameof(CreateFolderEndpoint);
}
