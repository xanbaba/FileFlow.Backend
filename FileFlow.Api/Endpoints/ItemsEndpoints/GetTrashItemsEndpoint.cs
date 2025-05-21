namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class GetTrashItemsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.ItemsEndpoints.GetTrashItems, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetTrashItemsEndpoint);
}
