using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetTrashItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetTrashItems, async (
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var items = await itemService.GetInTrashAsync(userId, cancellationToken);
                
                return Results.Ok(items.Select(item => item.ToResponse()));
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<IEnumerable<FileFolderResponse>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves items in trash",
                Description = "Returns a list of all files and folders that are currently in the user's trash.\n\n" +
                              "### Behavior\n" +
                              "- Returns only items that belong to the authenticated user\n" +
                              "- Returns both files and folders that have IsInTrash=true\n" +
                              "- Results are typically sorted by the date they were moved to trash\n" +
                              "- May include information about when items will be automatically permanently deleted\n\n" +
                              "### Response\n" +
                              "Returns an array of FileFolderResponse objects containing metadata about each trashed item."
            });
    }

    public string Name => nameof(GetTrashItemsEndpoint);
}