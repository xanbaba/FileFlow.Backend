using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetStarredItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetStarredItems, async (
                IItemService itemService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserid();
                var items = await itemService.GetStarredAsync(userId, cancellationToken);
                
                return Results.Ok(items.Select(item => item.ToResponse()));
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<IEnumerable<FileFolderResponse>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves all starred items",
                Description = "Returns a list of all files and folders that have been marked as starred by the user.\n\n" +
                              "### Behavior\n" +
                              "- Returns only items that belong to the authenticated user\n" +
                              "- Returns both files and folders that have IsStarred=true\n" +
                              "- Does not return items that are in trash, even if they are starred\n" +
                              "- Results are typically sorted by name or last modified date\n\n" +
                              "### Response\n" +
                              "Returns an array of FileFolderResponse objects containing metadata about each starred item."
            });
    }

    public string Name => nameof(GetStarredItemsEndpoint);
}