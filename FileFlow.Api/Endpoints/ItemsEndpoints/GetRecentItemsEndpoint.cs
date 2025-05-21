namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetRecentItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetRecentItems, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetRecentItemsEndpoint);
}
