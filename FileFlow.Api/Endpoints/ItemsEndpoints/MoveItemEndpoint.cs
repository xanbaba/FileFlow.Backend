namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class MoveItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.ItemsEndpoints.MoveItem, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(MoveItemEndpoint);
}
