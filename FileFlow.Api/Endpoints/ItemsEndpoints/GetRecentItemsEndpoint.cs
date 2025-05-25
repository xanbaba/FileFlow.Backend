using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetRecentItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetRecentItems, async (
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var items = await itemService.GetRecentAsync(userId, cancellationToken);
                
                return Results.Ok(items.Select(item => item.ToResponse()));
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<IEnumerable<FileFolderResponse>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves recently accessed items",
                Description = "Returns a list of files and folders that have been recently accessed or modified by the user.\n\n" +
                              "### Query Parameters\n" +
                              "- **limit** (int, optional): Maximum number of items to return. Default is typically 20.\n\n" +
                              "### Behavior\n" +
                              "- Returns only items that belong to the authenticated user\n" +
                              "- Returns both files and folders, sorted by last accessed or modified time, most recent first\n" +
                              "- Does not return items that are in trash\n" +
                              "- Typically includes items that were recently viewed, edited, uploaded, or downloaded\n\n" +
                              "### Response\n" +
                              "Returns an array of FileFolderResponse objects containing metadata about each recent item."
            });
    }

    public string Name => nameof(GetRecentItemsEndpoint);
}