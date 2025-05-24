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
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(GetRecentItemsEndpoint);
}