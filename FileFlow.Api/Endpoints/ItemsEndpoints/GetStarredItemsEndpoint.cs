namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetStarredItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetStarredItems, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetStarredItemsEndpoint);
}
