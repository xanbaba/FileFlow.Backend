using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.UserStorageEndpoints;

public class GetStorageEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.UserEndpoints.GetStorage,
                async (IUserStorageService userStorageService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
                {
                    var userId = user.GetUserid();
                    var userStorage = await userStorageService.GetAsync(userId, cancellationToken);
                    if (userStorage == null)
                    {
                        throw new Exception($"User storage not found for user {userId}");
                    }
                    return Results.Ok(userStorage);
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<UserStorageResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi(op => new(op)
            {
                Summary = "Retrieves user storage information",
                Description = "Returns details about the user's storage usage and limits.\n\n" +
                              "### Behavior\n" +
                              "- Returns storage information for the authenticated user\n" +
                              "- Calculates total used space across all the user's files\n" +
                              "- Provides a breakdown of storage usage by file category (documents, images, videos, other)\n" +
                              "- Includes the user's maximum allowed storage space\n\n" +
                              "### Response\n" +
                              "Returns a UserStorageResponse object containing information about storage usage, limits, and breakdown by file type.\n\n" +
                              "Numbers are in MB."
            });

    }

    public string Name => nameof(GetStorageEndpoint);
}