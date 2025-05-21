namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class UnstarItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.ItemsEndpoints.UnstarItem, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(UnstarItemEndpoint);
}
