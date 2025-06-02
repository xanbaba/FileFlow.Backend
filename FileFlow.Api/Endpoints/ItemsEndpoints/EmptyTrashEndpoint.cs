using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class EmptyTrashEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.TrashEndpoints.EmptyTrash,
                async (IItemService itemService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    await itemService.EmptyTrashAsync(user.GetUserid(), cancellationToken);
                    return Results.Ok();
                })
            .WithName(Name)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    public string Name => nameof(EmptyTrashEndpoint);
}