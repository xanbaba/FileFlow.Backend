using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class GetChildrenEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FolderEndpoints.GetChildren,
                async (string id, IFolderService folderService, ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    Guid? folderId = id == "root" ? null :
                        Guid.TryParse(id, out var guid) ? guid :
                        throw new BadHttpRequestException(
                            "Invalid folder id. Folder id must either be \"root\" or a valid id");
                    var children = await folderService.GetChildrenAsync(userId, folderId, cancellationToken);
                    return Results.Ok(children.Select(child => child.ToResponse()));
                }
            )
            .WithName(Name)
            .RequireAuthorization()
            .Produces<IEnumerable<FileFolderResponse>>()
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves all children (files and folders) of a specific folder",
                Description = "Retrieves a list of files and folders that are direct children of the specified folder.\n\n" +
                              "### Route Parameters\n" +
                              "- **id** (string): The ID of the folder to get children from. Use \"root\" to get children of the root folder, or provide a valid GUID for a specific folder.\n\n" +
                              "### Behavior\n" +
                              "- If id is \"root\", returns all items at the root level\n" +
                              "- If id is a valid folder GUID, returns all direct children of that folder\n" +
                              "- Returns only items that belong to the authenticated user\n" +
                              "- Does not return items that are in trash\n\n" +
                              "### Response\n" +
                              "Returns an array of FileFolderResponse objects containing metadata about each child item."
            });
    }

    public string Name => nameof(GetChildrenEndpoint);
}