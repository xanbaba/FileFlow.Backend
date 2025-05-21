namespace FileFlow.Api.Endpoints.TrashEndpoints;

public class EmptyTrashEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.TrashEndpoints.EmptyTrash, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(EmptyTrashEndpoint);
}
