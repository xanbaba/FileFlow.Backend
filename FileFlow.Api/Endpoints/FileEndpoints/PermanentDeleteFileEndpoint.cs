using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class PermanentDeleteFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FileEndpoints.PermanentDeleteFile,
                async (Guid id, IFileService fileService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    await fileService.DeletePermanentlyAsync(userId, id, cancellationToken);
                    return Results.NoContent();
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorMessage>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(PermanentDeleteFileEndpoint);
}