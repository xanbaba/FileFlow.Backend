namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetAllItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetAllItems, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetAllItemsEndpoint);
}
