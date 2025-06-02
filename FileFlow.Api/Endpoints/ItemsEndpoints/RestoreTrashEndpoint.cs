namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class RestoreTrashEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.TrashEndpoints.RestoreTrash,
            async () =>
            {

            })
            .WithName(Name)
            .RequireAuthorization()
            .WithOpenApi(op => new(op)
            {
                Summary = "Restore all items in trash",
                Description = "Restores all items in the user's trash.\n\n" +
                              "### Behavior\n" +
                              "- Only restores items that belong to the authenticated user\n" +
                              "- Sets the IsInTrash flag to false in the item metadata\n" +
                              "- If the item's original parent folder no longer exists or is in trash, the item will be moved to root\n" +
                              "### Response\n" +
                              "Returns 200 Ok if successful."
            });
    }

    public string Name => nameof(RestoreTrashEndpoint);
}